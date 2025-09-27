using CarCostCalculator_App.EF.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCostCalculator_App.EF.Entities
{
    [PrimaryKey(nameof(Id))]
    public class MonthlyData
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public double TotalCost => CostEntries.Sum(c => c.Price);
        public double TotalKilometers => KilometerEntries.Sum(k => k.Kilometers);

        [ForeignKey(nameof(AnnualDataId))]
        public int AnnualDataId { get; set; }
        public AnnualData? AnnualData { get; set; }

        public ICollection<CostEntry> CostEntries { get; set; } = [];
        public ICollection<KilometerEntry> KilometerEntries { get; set; } = [];

    }
}
