using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCostCalculator_App.EF.Entities
{
    [PrimaryKey(nameof(Id))]
    public class KilometerEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Kilometers { get; set; }

        [ForeignKey(nameof(MonthlyDataId))]
        public int MonthlyDataId { get; set; }
        public MonthlyData? MonthlyData { get; set; }
    }
}
