using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Data.Contract
{
    public interface IKilometerEntryRepository
    {
        Task<KilometerEntryCore?> Create(KilometerEntryCore kilometerEntry, CancellationToken cancellationToken);

        Task<KilometerEntryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default);
    }
}
