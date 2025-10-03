using Car_Cost_Calculator_App.API;
using Car_Cost_Calculator_App.Shared;
using CarCostCalculator_App.CCL.CQRS.HTTP.Client;
using CarCostCalculator_App.Data.Contract;
using CarCostCalculator_App.Data.Repository;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Logic;
using CarCostCalculator_App.EF;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OfficeOpenXml;

namespace Car_Cost_Calculator_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Personal");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddMudServices();
            builder.Services.AddMudBlazorDialog();

            // add repos
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


            builder.Services
                .AddCommandQueryClient(cfg => cfg.RegisterContractsFromAssembly(typeof(CategoryByPrimaryKey).Assembly))
                .AddHttpClient(cfg =>
                {
                    cfg.BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"] ?? throw new InvalidOperationException("ApiBaseAddress is not configured."));
                });
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CategoryQueryHandler).Assembly));

            builder.Services.AddSingleton<AppThemes>();


            var conntectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CarCostCalculatorContext>(options =>
                options.UseSqlServer(conntectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
