using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.Metadata;
using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Domain.Contract.CostEntry
{
    [Metadata(nameof(CostEntry))]
    public record CostEntryByPrimaryKey(int Id) : IQuery<CostEntryCore?>;
}
