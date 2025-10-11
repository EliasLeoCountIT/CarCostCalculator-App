using AutoMapper;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Model;
using CarCostCalculator_App.EF;
using CarCostCalculator_App.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator_App.Data.Repository
{
    public class CategoryRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<Category, CategoryCore>(context, mapper), ICategoryRepository
    {
        public async Task<CategoryCore?> Create(CategoryCore category, CancellationToken cancellationToken = default)
        {
            if (category is null)
            {
                throw new InvalidDataException("Category is null");
            }

            var entity = Mapper.Map<Category>(category);

            if (await Context.Categories.AnyAsync(c => c.Name.Equals(entity.Name), cancellationToken))
            {
                throw new Exception($"Eine Kategorie mit dem Namen '{entity.Name}' existiert bereits.");
            }

            if (await Context.Categories.AnyAsync(c => c.Id == entity.Id, cancellationToken))
            {
                return await LoadByPrimaryKey(entity.Id, cancellationToken);
            }

            await Context.Categories.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return await LoadByPrimaryKey(entity.Id, cancellationToken) ?? throw new KeyNotFoundException($"No Category with ID: {entity.Id} found in DB after adding");
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            var categoryToRemove = await Context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (categoryToRemove is not null)
            {
                Context.Categories.Remove(categoryToRemove);
                await Context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new KeyNotFoundException($"No Category with ID: {id} found in DB.");
            }
        }

        public async Task<CategoryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default)
        {
            var entity = await QueryEntities().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return entity == null
                ? throw new KeyNotFoundException($"No Category with ID: {id} found in DB.")
                : Mapper.Map<CategoryCore>(entity);
        }

        public async Task<CategoryCore?> Update(CategoryCore category, CancellationToken cancellationToken = default)
        {
            var existingEntity = await Context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id, cancellationToken);

            if (existingEntity != null)
            {
                Mapper.Map(category, existingEntity);

                await Context.SaveChangesAsync(cancellationToken);

                return await LoadByPrimaryKey(existingEntity.Id, cancellationToken);
            }

            return null;
        }
    }
}
