using CarCostCalculator_App.CCL.Contracts;
using CarCostCalculator_App.CCL.Metadata;
using DTO = CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Domain.Contract.KilometerEntry
{
    [Metadata(nameof(KilometerEntry))]
    public record AddKilometerEntry(DTO.KilometerEntryCore KilometerEntry) : ICommand<DTO.KilometerEntryCore?>;

}
