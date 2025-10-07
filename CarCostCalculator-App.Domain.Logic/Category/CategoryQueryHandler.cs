using CarCostCalculator_App.CCL.CQRS.Extensions;
using CarCostCalculator_App.CCL.CQRS.Queries;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Model;
using MediatR;
using System.Linq.Expressions;

namespace CarCostCalculator_App.Domain.Logic.Category;

public class CategoryQueryHandler(ICategoryRepository repository)
    : ODataBaseHandler<CategoriesViaOData, CategoryCore>,
      IRequestHandler<CategoryByPrimaryKey, CategoryCore?>
{
    ICategoryRepository _repository = repository;

    public async Task<CategoryCore?> Handle(CategoryByPrimaryKey request, CancellationToken cancellationToken)
        => await _repository.LoadByPrimaryKey(request.Id, cancellationToken);

    protected override IQueryable<CategoryCore> RetrieveQueryable(CategoriesViaOData request)
    {
        var query = _repository.QueryProjection();

        if (request.Terms is null || request.Terms.Length == 0)
        {
            return query;
        }

        var properties = new List<Expression<Func<CategoryCore, string?>>>
        {
            c => c.Name
        };

        return query.ApplySearch(request.Terms, properties);
    }
}
