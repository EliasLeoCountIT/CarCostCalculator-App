using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarCostCalculator_App.EF.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public required string Name { get; set; }
    }
}