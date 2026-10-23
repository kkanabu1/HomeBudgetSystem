using System.ComponentModel.DataAnnotations;

namespace HomeBudgetSystem.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal MonthlyLimit { get; set; }
    }
}