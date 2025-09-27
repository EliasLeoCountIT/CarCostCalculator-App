using Microsoft.EntityFrameworkCore;

namespace CarCostCalculator_App.EF.Entities
{
    [PrimaryKey(nameof(Id))]
    public class AnnualData
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public ICollection<MonthlyData> MonthlyData { get; set; } = [];
    }
}
