using CarCostCalculator_App.CCL.AspNetCore;
using CarCostCalculator_App.CCL.AspNetCore.Tracking;
using CarCostCalculator_App.CCL.CQRS;
using CarCostCalculator_App.CCL.CQRS.AspNetCore;
using CarCostCalculator_App.CCL.CQRS.EntityFrameworkCore;
using CarCostCalculator_App.Data.Repository.Extensions;
using CarCostCalculator_App.Domain.Contract.Category;
using CarCostCalculator_App.Domain.Logic.Category;
using CarCostCalculator_App.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Car_Cost_Calculator_App.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplication? app = null;
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                if (app!.Environment.IsDevelopment())
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        _ = policy
                            .SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                }
            });



            builder.Services.AddDbContext<CarCostCalculatorContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSingleton(_ => AutoMapperConfiguration.Configure().CreateMapper());

            builder.Services.AddRepositories();
            builder.Services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CategoryQueryHandler).Assembly))
                .AddRequestContext()
                .AddCommandQueryEndpoints(cfg => cfg.RegisterContractsFromAssembly(typeof(CategoryByPrimaryKey).Assembly))
                .AddODataEndpoints()
                .AddAsyncBehavior()
                ;
            ;

            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(c =>
                            {
                                c.UseAllOfForInheritance();
                                c.OperationFilter<AddDataOriginHeaderAsSwaggerParameter>("x-data-origin");
                            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Car-Cost-Calculator_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });



            builder.Services.AddHttpContextAccessor();

            app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
                _ = app.UseDeveloperExceptionPage();
                _ = app.UseMigrationsEndPoint();
            }

            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());



            app.MapCommandQueryEndpoints();

            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());

            app.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });



            await app.RunAsync();
        }
    }
}
