using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Data.Contract
{
    public interface IKilometerEntryRepository
    {
        Task<KilometerEntry?> Create(KilometerEntry kilometerEntry, CancellationToken cancellationToken);

        Task<KilometerEntry?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default);
    }
}
