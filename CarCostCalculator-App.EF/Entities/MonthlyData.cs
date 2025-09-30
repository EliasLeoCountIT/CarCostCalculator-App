using CarCostCalculator_App.Domain.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCostCalculator_App.EF.Entities
{
    [PrimaryKey(nameof(Id))]
    public class MonthlyData
    {
        public int Id { get; set; }
        public Month Month { get; set; }

        [ForeignKey(nameof(AnnualDataId))]
        public int AnnualDataId { get; set; }
        public AnnualData? AnnualData { get; set; }

        public ICollection<CostEntry> CostEntries { get; set; } = [];
        public ICollection<KilometerEntry> KilometerEntries { get; set; } = [];

    }
}
