using CarCostCalculator_App.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator_App.EF
{
    public class CarCostCalculatorContext : DbContext
    {
        public CarCostCalculatorContext(DbContextOptions<CarCostCalculatorContext> options)
            : base(options) { }

        public DbSet<AnnualData> AnnualDatas => Set<AnnualData>();
        public DbSet<MonthlyData> MonthlyDatas => Set<MonthlyData>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<CostEntry> CostEntries => Set<CostEntry>();
        public DbSet<KilometerEntry> KilometerEntries => Set<KilometerEntry>();
    }
}
