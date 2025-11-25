using BudgetTracker.Core.Enums;

namespace BudgetTracker.Application.DTOs;

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
