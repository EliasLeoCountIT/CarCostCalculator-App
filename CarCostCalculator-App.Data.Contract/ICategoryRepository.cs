using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Data.Contract
{
    public interface ICategoryRepository
    {
        Task<Category?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default);
    }
}
