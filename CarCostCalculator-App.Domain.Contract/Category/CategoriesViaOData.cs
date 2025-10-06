using CarCostCalculator_App.CCL.CQRS.Abstractions;
using CarCostCalculator_App.CCL.Metadata;
using DTO = CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Domain.Contract.Category;

[Metadata(nameof(Category))]
public record CategoriesViaOData : ODataSearchBaseQuery<DTO.CategoryCore>;
