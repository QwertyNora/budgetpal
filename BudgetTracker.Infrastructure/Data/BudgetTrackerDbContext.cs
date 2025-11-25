using Microsoft.EntityFrameworkCore;
using BudgetTracker.Core.Entities;
using BudgetTracker.Core.Enums;

namespace BudgetTracker.Infrastructure.Data;

public class BudgetTrackerDbContext : DbContext
{
    public BudgetTrackerDbContext(DbContextOptions<BudgetTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=budgettracker.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Type)
                .IsRequired();

            entity.Property(c => c.IsCustom)
                .IsRequired();

            entity.Property(c => c.CreatedAt)
                .IsRequired();

            // Unique index on Name
            entity.HasIndex(c => c.Name)
                .IsUnique();

            // Configure relationship
            entity.HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Transaction entity
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Date)
                .IsRequired();

            entity.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            entity.Property(t => t.Type)
                .IsRequired();

            entity.Property(t => t.CategoryId)
                .IsRequired();

            entity.Property(t => t.Notes)
                .HasMaxLength(500);

            entity.Property(t => t.CreatedAt)
                .IsRequired();
        });

        // Seed predefined categories
        var categories = new List<Category>();
        var categoryId = 1;
        var createdAt = new DateTime(2025, 11, 25, 0, 0, 0, DateTimeKind.Utc);

        // Income categories
        var incomeCategories = new[] { "Salary", "Freelance", "Investment Returns", "Gifts", "Other Income" };
        foreach (var name in incomeCategories)
        {
            categories.Add(new Category
            {
                Id = categoryId++,
                Name = name,
                Type = CategoryType.Income,
                IsCustom = false,
                CreatedAt = createdAt
            });
        }

        // Expense categories
        var expenseCategories = new[]
        {
            "Groceries", "Dining Out", "Transportation", "Utilities", "Rent/Mortgage",
            "Healthcare", "Insurance", "Entertainment", "Shopping", "Education",
            "Travel", "Personal Care", "Subscriptions", "Gifts & Donations",
            "Home Maintenance", "Pet Care", "Other Expenses"
        };
        foreach (var name in expenseCategories)
        {
            categories.Add(new Category
            {
                Id = categoryId++,
                Name = name,
                Type = CategoryType.Expense,
                IsCustom = false,
                CreatedAt = createdAt
            });
        }

        // Both category
        categories.Add(new Category
        {
            Id = categoryId++,
            Name = "Miscellaneous",
            Type = CategoryType.Both,
            IsCustom = false,
            CreatedAt = createdAt
        });

        modelBuilder.Entity<Category>().HasData(categories);
    }
}
