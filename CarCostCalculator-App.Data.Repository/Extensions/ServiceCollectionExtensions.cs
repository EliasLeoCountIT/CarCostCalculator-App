using CarCostCalculator_App.Data.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace CarCostCalculator_App.Data.Repository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IKilometerEntryRepository, KilometerEntryRepository>();

            return services;
        }
    }
}
