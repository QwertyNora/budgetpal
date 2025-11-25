using System.ComponentModel.DataAnnotations;
using BudgetTracker.Core.Enums;

namespace BudgetTracker.Core.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public CategoryType Type { get; set; }

    public bool IsCustom { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation property
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
