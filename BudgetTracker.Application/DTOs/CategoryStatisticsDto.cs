using BudgetTracker.Core.Enums;

namespace BudgetTracker.Application.DTOs;

public class CategoryStatisticsDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public decimal TotalAmount { get; set; }
    public int TransactionCount { get; set; }
}
