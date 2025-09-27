using CarCostCalculator_App.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator_App.EF
{
    public class CarCostCalculatorContext : DbContext
    {
        public CarCostCalculatorContext(DbContextOptions<CarCostCalculatorContext> options)
            : base(options) { }

        public DbSet<AnnualData> AnnualDatas { get; set; }
        public DbSet<MonthlyData> MonthlyDatas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CostEntry> CostEntries { get; set; }
        public DbSet<KilometerEntry> KilometerEntries { get; set; }
    }
}
