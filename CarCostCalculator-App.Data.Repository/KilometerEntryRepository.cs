using AutoMapper;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Model;
using CarCostCalculator_App.EF;
using Microsoft.EntityFrameworkCore;
using DTO = CarCostCalculator_App.Domain.Model;
using Entities = CarCostCalculator_App.EF.Entities;

namespace CarCostCalculator_App.Data.Repository
{
    public class KilometerEntryRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<Entities.KilometerEntry, DTO.KilometerEntry>(context, mapper), IKilometerEntryRepository
    {
        public async Task<KilometerEntry?> Create(KilometerEntry kilometerEntry, CancellationToken cancellationToken)
        {
            if (kilometerEntry is null)
            {
                throw new InvalidDataException("KilometerEntry is null");
            }

            var entity = Mapper.Map<Entities.KilometerEntry>(kilometerEntry);

            if (await Context.KilometerEntries.AnyAsync(k => k.Id == entity.Id, cancellationToken))
            {
                return await LoadByPrimaryKey(entity.Id, cancellationToken);
            }

            await Context.KilometerEntries.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return await LoadByPrimaryKey(entity.Id, cancellationToken) ?? throw new KeyNotFoundException($"No KilometerEntry with Id: {entity.Id} found in DB after adding");
        }

        public async Task<KilometerEntry?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default)
        {
            var entity = await QueryEntities().FirstOrDefaultAsync(k => k.Id == id, cancellationToken);

            return entity == null
                ? throw new KeyNotFoundException($"No KilometerEntry with ID: {id} found in DB")
                : Mapper.Map<KilometerEntry>(entity);
        }
    }
}
