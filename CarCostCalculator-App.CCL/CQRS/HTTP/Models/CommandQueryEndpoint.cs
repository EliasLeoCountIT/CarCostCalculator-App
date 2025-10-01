using CarCostCalculator_App.CCL.Metadata;

namespace CarCostCalculator_App.CCL.CQRS.HTTP.Models
{
    /// <summary>
    /// Represents an Command/Query endpoint.
    /// </summary>
    /// <param name="Name">Name of the endpoint.</param>
    /// <param name="Route">Route of the endpoint.</param>
    /// <param name="HttpMethod"><see cref="System.Net.Http.HttpMethod"/></param>
    /// <param name="Metadata"><see cref="MetadataAttribute"/></param>
    public record CommandQueryEndpoint(string Name, string Route, HttpMethod HttpMethod, MetadataAttribute? Metadata = null);
}
