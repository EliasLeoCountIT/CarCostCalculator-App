using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;

namespace CarCostCalculator_App.EF
{
    public class CarCostCalculatorDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CarCostCalculatorContext>
    {
        #region Public Methods

        public CarCostCalculatorContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true)
               .Build();

            var connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;

            var optionsBuilder = !string.IsNullOrEmpty(connectionString)
                ? new DbContextOptionsBuilder<CarCostCalculatorContext>().UseSqlServer(connectionString, builder => builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName))
                : new DbContextOptionsBuilder<CarCostCalculatorContext>().UseSqlServer(builder => builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName));

            return new CarCostCalculatorContext(optionsBuilder.Options);
        }

        #endregion
    }
}