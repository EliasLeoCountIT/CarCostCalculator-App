using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarCostCalculator_App.EF.Entities
{
    [PrimaryKey(nameof(Id))]
    public class CostEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [ForeignKey(nameof(MonthlyDataId))]
        public int MonthlyDataId { get; set; }
        public MonthlyData? MonthlyData { get; set; }
    }
}
