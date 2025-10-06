namespace CarCostCalculator_App.Domain.Model
{
    public class MonthlyDataCore
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int AnnualDataId { get; set; }
        public double TotalCost { get; set; }
        public double TotalKilometers { get; set; }

        public List<CostEntryCore> CostEntries { get; set; } = [];
        public List<KilometerEntryCore> KilometerEntries { get; set; } = [];

    }
}
