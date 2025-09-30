using CarCostCalculator_App.Domain.Model.Enums;

namespace CarCostCalculator_App.Domain.Model
{
    public class MonthlyData
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public int AnnualDataId { get; set; }
        public double TotalCost { get; set; }
        public double TotalKilometers { get; set; }

        public List<CostEntry> CostEntries { get; set; } = [];
        public List<KilometerEntry> KilometerEntries { get; set; } = [];

    }
}
