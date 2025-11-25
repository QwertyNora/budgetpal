using BudgetTracker.Core.Enums;

namespace BudgetTracker.Application.DTOs;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
