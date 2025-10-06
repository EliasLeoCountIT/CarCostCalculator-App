namespace CarCostCalculator_App.Domain.Model
{
    public class CostEntryCore
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public int MonthlyDataId { get; set; }
    }
}
