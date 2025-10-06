using AutoMapper;
using Car_Cost_Calculator_App.API;
using CarCostCalculator_App.Domain.Model;
using CarCostCalculator_App.EF.Entities;

namespace CarCostCalculator_App.Data.Repository.Test
{
    public class MonthlyDataRepositoryTest
    {
        [Fact]
        public void Mapping_MonthlyData_To_MonthlyDataCore_Sums_Kilometers()
        {
            var monthlyData = new MonthlyData
            {
                Id = 1,
                KilometerEntries =
                                        [
                                            new KilometerEntry { Id=1, Kilometers = 50, PaymentDate =DateTime.Now.AddDays(-2) },
                                            new KilometerEntry { Id=2, Kilometers = 70, PaymentDate =DateTime.Now.AddDays(-5)  }
                                        ]
            };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var mapper = config.CreateMapper();
            var core = mapper.Map<MonthlyDataCore>(monthlyData);

            Assert.Equal(120, core.TotalKilometers);
        }
    }
}
