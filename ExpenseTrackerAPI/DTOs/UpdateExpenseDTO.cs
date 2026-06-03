namespace ExpenseTrackerAPI.DTOs
{
    public class UpdateExpenseDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
