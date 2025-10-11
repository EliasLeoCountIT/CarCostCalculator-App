using AutoMapper;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Domain.Model;
using CarCostCalculator_App.EF;
using CarCostCalculator_App.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator_App.Data.Repository
{
    public class CostEntryRepository(CarCostCalculatorContext context, IMapper mapper) : BaseRepository<CostEntry, CostEntryCore>(context, mapper), ICostEntryRepository
    {
        public async Task<CostEntryCore?> Create(CostEntryCore costEntry, CancellationToken cancellationToken)
        {

            if (costEntry is null)
            {
                throw new InvalidDataException("CostEntry is null");
            }

            var year = costEntry.PaymentDate.Year;
            var month = costEntry.PaymentDate.Month;

            // 1. AnnualData suchen oder anlegen
            var annualData = await Context.AnnualDatas
                  .Include(a => a.MonthlyData)
                  .FirstOrDefaultAsync(a => a.Year == year, cancellationToken);

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

            costEntry.MonthlyDataId = monthlyData.Id;
            var entity = Mapper.Map<CostEntry>(costEntry);

            if (await Context.CostEntries.AnyAsync(k => k.Id == entity.Id, cancellationToken))
            {
                return await LoadByPrimaryKey(entity.Id, cancellationToken);
            }

            await Context.CostEntries.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            return await LoadByPrimaryKey(entity.Id, cancellationToken) ?? throw new KeyNotFoundException($"No CostEntry with Id: {entity.Id} found in DB after adding");
        }

        public async Task<CostEntryCore?> LoadByPrimaryKey(int id, CancellationToken cancellationToken = default)
        {
            var entity = await QueryEntities().FirstOrDefaultAsync(k => k.Id == id, cancellationToken);

            return entity == null
                ? throw new KeyNotFoundException($"No CostEntry with ID: {id} found in DB")
                : Mapper.Map<CostEntryCore>(entity);
        }
    }
}
