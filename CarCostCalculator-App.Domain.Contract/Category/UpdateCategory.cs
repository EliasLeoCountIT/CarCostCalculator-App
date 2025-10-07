using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.Metadata;
using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Domain.Contract.Category;


[Metadata(nameof(Category))]
public record UpdateCategory(CategoryCore Category) : IIdempotentCommand<CategoryCore?>;
