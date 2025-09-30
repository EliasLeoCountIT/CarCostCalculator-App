using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Contract;
using CarCostCalculator_App.Domain.Model;
using MediatR;

namespace CarCostCalculator_App.Domain.Logic;

public class CategoryQueryHandler(ICategoryRepository repository)
    : IRequestHandler<CategoryByPrimaryKey, Category?>
{
    ICategoryRepository _repository = repository;

    public async Task<Category?> Handle(CategoryByPrimaryKey request, CancellationToken cancellationToken)
        => await _repository.LoadByPrimaryKey(request.Id, cancellationToken);
}
