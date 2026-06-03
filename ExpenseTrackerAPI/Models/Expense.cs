using System;

namespace ExpenseTrackerAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // Foreign key property
        public int UserId { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
