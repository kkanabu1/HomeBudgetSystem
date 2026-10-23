using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeBudgetSystem.Web.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public bool IsIncome { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}