namespace ExpenseTrackerAPI.DTOs
{
    public class CreateExpenseDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
