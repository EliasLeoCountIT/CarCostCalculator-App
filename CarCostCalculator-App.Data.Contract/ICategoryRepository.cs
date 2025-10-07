using CarCostCalculator_App.Domain.Model;

namespace CarCostCalculator_App.Data.Contract
{
    public interface ICategoryRepository : IQueryableRepository<CategoryCore>
    {
        Task<CategoryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default);

        Task Delete(int id, CancellationToken cancellationToken = default);

        Task<CategoryCore?> Update(CategoryCore category, CancellationToken cancellationToken = default);

        Task<CategoryCore?> Create(CategoryCore category, CancellationToken cancellationToken = default);
    }
}
