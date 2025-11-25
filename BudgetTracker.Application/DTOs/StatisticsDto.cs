namespace BudgetTracker.Application.DTOs;

public class StatisticsDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance { get; set; }
    public int TransactionCount { get; set; }
}
