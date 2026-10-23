using System.ComponentModel.DataAnnotations;

namespace HomeBudgetSystem.Web.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal PlannedIncome { get; set; }

        [Required]
        public decimal PlannedExpenses { get; set; }
    }
}