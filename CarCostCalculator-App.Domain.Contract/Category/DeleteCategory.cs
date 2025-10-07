using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.Metadata;

namespace CarCostCalculator_App.Domain.Contract.Category;

[Metadata(nameof(Category))]
public record DeleteCategory(int Id) : IDeleteCommand;

