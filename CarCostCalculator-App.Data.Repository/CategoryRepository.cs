using AutoMapper;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.EF;
using Microsoft.EntityFrameworkCore;
using DTO = CarCostCalculator_App.Domain.Model;
using Entities = CarCostCalculator_App.EF.Entities;

namespace CarCostCalculator_App.Data.Repository
{
    public class CategoryRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<Entities.Category, DTO.CategoryCore>(context, mapper), ICategoryRepository
    {
        public async Task<DTO.CategoryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default)
        {
            var entity = await QueryEntities().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return entity == null
                ? throw new KeyNotFoundException($"No Category with ID: {id} found in DB.")
                : Mapper.Map<DTO.CategoryCore>(entity);
        }
    }
}
