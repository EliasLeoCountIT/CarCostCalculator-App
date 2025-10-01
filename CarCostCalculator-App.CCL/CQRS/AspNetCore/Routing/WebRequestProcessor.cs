using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.OData;
using CarCostCalculator_App.CCL.CQRS.AspNetCore.Utilities;
using CarCostCalculator_App.CCL.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OData.Edm;

namespace CarCostCalculator_App.CCL.CQRS.AspNetCore.Routing
{
    /// <summary>
    /// Processing incoming web requests from Command/Query endpoints.
    /// </summary>
    /// <param name="logger">Represents a type used to perform logging.</param>
    /// <param name="mediationSender">Defines a mediator pipeline sender.</param>
    /// <param name="serviceProvider">Defines a mechanism for retrieving service objects.</param>
    public class WebRequestProcessor(ILogger<WebRequestProcessor> logger, ISender mediationSender, IServiceProvider serviceProvider)
    {
        #region Private Members

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        #endregion

        #region Public Properties

        /// <summary>
        /// The logger used for logging information, warnings, and errors.
        /// </summary>
        public ILogger<WebRequestProcessor> Logger { get; } = logger;

        /// <summary>
        /// The active mediator pipeline sender.
        /// </summary>
        public ISender MediationSender { get; } = mediationSender;

        #endregion

        #region Public Methods

        /// <summary>
        /// Executes the <see cref="IODataQuery{TItem}"/> and returns a <see cref="PagedDataResult{TItem}"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the requested items.</typeparam>
        /// <param name="oDataQuery">The current <see cref="IODataQuery{TItem}"/>.</param>
        /// <param name="request">Represents the incoming side of the current HTTP request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning a <see cref="PagedDataResult{TItem}"/>.</returns>
        public async Task<PagedDataResult<TItem>> ExecuteODataQuery<TItem>(IODataQuery<TItem> oDataQuery, HttpRequest request, CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Execute oData query: {oDataQuery}", oDataQuery);

            var queryable = await MediationSender.Send(oDataQuery, cancellationToken);
            var queryOptions = BuildODataOptions(oDataQuery, request);

            var odataFeature = request.ODataFeature();
            var resultQueryable = queryOptions.ApplyTo(queryable,
                                                       new ODataQuerySettings
                                                       {
                                                           HandleNullPropagation = HandleNullPropagationOption.Default,
                                                           EnableConstantParameterization = true,
                                                           EnsureStableOrdering = true,
                                                           TimeZone = TimeZoneInfo.Utc
                                                       }).Cast<TItem>();

            return await MediationSender.Send(new ODataResultRequest<TItem>(oDataQuery, resultQueryable, odataFeature.TotalCount), cancellationToken);
        }

        /// <summary>
        /// Processing common <see cref="IWebRequest"/>.
        /// </summary>
        /// <typeparam name="TRequest">The type of the common <see cref="IWebRequest"/>.</typeparam>
        /// <param name="request">The current <see cref="IWebRequest"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <see cref="IResult"/>.</returns>
        public async Task<IResult> HandleCommonRequest<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IWebRequest
        {
            ArgumentNullException.ThrowIfNull(request);

            Logger.LogInformation("Processing request: {Request}", request);

            var response = await MediationSender.Send(request, cancellationToken);

            return response switch
            {
                IEnumerable<object?> items => OkOrNoContent(items),
                PagedDataResult result => OkOrNoContent(result),
                _ => OkOrNotFound(request, response)
            };
        }

        /// <summary>
        /// Processing <see cref="IFileQuery"/>.
        /// </summary>
        /// <param name="query">The current <see cref="IFileQuery"/>.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <see cref="IResult"/>.</returns>
        public async Task<IResult> HandleFileQuery(IFileQuery query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            Logger.LogInformation("Processing file query: {Query}", query);

            var response = await MediationSender.Send(query, cancellationToken);

            return FileOrNotFound(query, response);
        }

        /// <summary>
        /// Processing <see cref="IODataQuery{TItem}"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the requested items.</typeparam>
        /// <param name="query">The current <see cref="IODataQuery{TItem}"/>.</param>
        /// <param name="request">Represents the incoming side of the current HTTP request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Awaitable task returning the <see cref="IResult"/>.</returns>
        public async Task<IResult> HandleODataQuery<TItem>(IODataQuery<TItem> query, HttpRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            Logger.LogInformation("Processing oData query: {Query}", query);

            var result = await ExecuteODataQuery(query, request, cancellationToken);

            return OkOrNoContent(result);
        }

        #endregion

        #region Private Methods

        private static IResult FileOrNotFound(IFileQuery request, FileQueryItem? response)
            => response is not null
                ? Results.File(fileStream: response.FileStream,
                               contentType: response.MimeType,
                               fileDownloadName: response.DownloadName,
                               lastModified: response.LastModified,
                               entityTag: response.GetEntityTag())
                : NotFound(request);

        private static IResult NotFound(IWebRequest request)
            => Results.Problem(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Requested resource not found: {request}",
                Status = StatusCodes.Status404NotFound
            });

        private static IResult OkOrNoContent<TItem>(IEnumerable<TItem>? items)
            => items?.Any() ?? false
                ? Results.Ok(items)
                : Results.NoContent();

        private static IResult OkOrNoContent(PagedDataResult result)
            => (result?.TotalCount ?? 0) > 0
                ? Results.Ok(result)
                : Results.NoContent();

        private static IResult OkOrNotFound<TResponse>(IWebRequest request, TResponse response)
            => response is not null
                ? Results.Ok(response)
                : NotFound(request);

        private ODataQueryOptions<TResponse> BuildODataOptions<TResponse>(IODataQuery<TResponse> query, HttpRequest request)
        {
            var edmModel = _serviceProvider.GetRequiredService<IEdmModel>();
            var oDataQueryContext = new ODataQueryContext(edmModel, typeof(TResponse), null);

            var queryParams = new Dictionary<string, StringValues>(request.Query);

            queryParams.TryAdd("$count", "true");

            if (!queryParams.ContainsKey(nameof(query.Top)))
            {
                queryParams.TryAdd("$top", query.Top.ToString());
            }

            if (!queryParams.ContainsKey(nameof(query.Skip)))
            {
                queryParams.TryAdd("$skip", query.Skip.ToString());
            }

            if (!queryParams.ContainsKey(nameof(query.Filter)) && query.Filter is not null)
            {
                queryParams.TryAdd("$filter", query.Filter);
            }

            if (!queryParams.ContainsKey(nameof(query.OrderBy)) && query.OrderBy is not null)
            {
                queryParams.TryAdd("$orderby", query.OrderBy);
            }

            var queryCollection = new QueryCollection(queryParams);

            request.Query = queryCollection;
            return new ODataQueryOptions<TResponse>(oDataQueryContext, request);
        }

        #endregion
    }
}
