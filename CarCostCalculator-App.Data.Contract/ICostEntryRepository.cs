using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Data.Contract
{
    public interface ICostEntryRepository
    {
        Task<CostEntryCore?> Create(CostEntryCore costEntry, CancellationToken cancellationToken);

        Task<CostEntryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default);
    }
}
