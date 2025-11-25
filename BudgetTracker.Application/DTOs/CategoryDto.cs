using BudgetTracker.Core.Enums;

namespace BudgetTracker.Application.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; }
}
