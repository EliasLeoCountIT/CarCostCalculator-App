
namespace CarCostCalculator_App.Domain.Model
{
    public class AnnualData
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public double TotalCost { get; set; }
        public double TotalKilometers { get; set; }
        public List<MonthlyData> MonthlyData { get; set; } = [];
    }
}
