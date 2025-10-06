using AutoMapper;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Model;
using CarCostCalculator_App.EF;
using CarCostCalculator_App.EF.Entities;
using Microsoft.EntityFrameworkCore;
using DTO = CarCostCalculator_App.Domain.Model;
using Entities = CarCostCalculator_App.EF.Entities;

namespace CarCostCalculator_App.Data.Repository
{
    public class KilometerEntryRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<Entities.KilometerEntry, DTO.KilometerEntryCore>(context, mapper), IKilometerEntryRepository
    {
        public async Task<KilometerEntryCore?> Create(KilometerEntryCore kilometerEntry, CancellationToken cancellationToken)
        {

            if (kilometerEntry is null)
            {
                throw new InvalidDataException("KilometerEntry is null");
            }

            var year = kilometerEntry.PaymentDate.Year;
            var month = kilometerEntry.PaymentDate.Month;

            // 1. AnnualData suchen oder anlegen
            var annualData = await Context.AnnualDatas
                  .Include(a => a.MonthlyData)
                  .FirstOrDefaultAsync(a => a.Year == year);

            if (annualData == null)
            {
                annualData = new AnnualData { Year = year };
                await Context.AnnualDatas.AddAsync(annualData, cancellationToken);
                await Context.SaveChangesAsync(cancellationToken);
            }

            // 2. MonthlyData suchen oder anlegen
            var monthlyData = annualData.MonthlyData.FirstOrDefault(m => m.Month == month);

            if (monthlyData == null)
            {
                monthlyData = new MonthlyData { Month = month, AnnualDataId = annualData.Id };
                await Context.MonthlyDatas.AddAsync(monthlyData, cancellationToken);
                await Context.SaveChangesAsync(cancellationToken);
            }

            kilometerEntry.MonthlyDataId = monthlyData.Id;
            var entity = Mapper.Map<Entities.KilometerEntry>(kilometerEntry);

            if (await Context.KilometerEntries.AnyAsync(k => k.Id == entity.Id, cancellationToken))
            {
                return await LoadByPrimaryKey(entity.Id, cancellationToken);
            }

            await Context.KilometerEntries.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return await LoadByPrimaryKey(entity.Id, cancellationToken) ?? throw new KeyNotFoundException($"No KilometerEntry with Id: {entity.Id} found in DB after adding");
        }

        public async Task<KilometerEntryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default)
        {
            var entity = await QueryEntities().FirstOrDefaultAsync(k => k.Id == id, cancellationToken);

            return entity == null
                ? throw new KeyNotFoundException($"No KilometerEntry with ID: {id} found in DB")
                : Mapper.Map<KilometerEntryCore>(entity);
        }
    }
}
