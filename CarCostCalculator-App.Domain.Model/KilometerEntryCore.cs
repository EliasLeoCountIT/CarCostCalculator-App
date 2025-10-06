namespace CarCostCalculator_App.Domain.Model
{
    public class KilometerEntryCore
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Kilometers { get; set; }
        public int MonthlyDataId { get; set; }
    }
}
