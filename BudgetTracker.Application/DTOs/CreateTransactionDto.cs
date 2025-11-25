using BudgetTracker.Core.Enums;

namespace BudgetTracker.Application.DTOs;

public class CreateTransactionDto
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public int CategoryId { get; set; }
    public string? Notes { get; set; }
}
