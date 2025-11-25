using System.Globalization;
using BudgetTracker.Application.DTOs;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Core.Entities;
using BudgetTracker.Core.Enums;
using BudgetTracker.Core.Interfaces;

namespace BudgetTracker.Application.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Category> _categoryRepository;

    public StatisticsService(
        IRepository<Transaction> transactionRepository,
        IRepository<Category> categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public StatisticsDto GetOverallStatistics(DateTime? startDate, DateTime? endDate)
    {
        var transactions = FilterByDateRange(_transactionRepository.GetAll(), startDate, endDate);

        var totalIncome = transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

        var totalExpenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        return new StatisticsDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Balance = totalIncome - totalExpenses,
            TransactionCount = transactions.Count()
        };
    }

    public List<CategoryStatisticsDto> GetByCategory(DateTime? startDate, DateTime? endDate)
    {
        var transactions = FilterByDateRange(_transactionRepository.GetAll(), startDate, endDate).ToList();
        var categories = _categoryRepository.GetAll().ToDictionary(c => c.Id);

        var categoryStats = transactions
            .GroupBy(t => t.CategoryId)
            .Select(g => new CategoryStatisticsDto
            {
                CategoryId = g.Key,
                CategoryName = categories.ContainsKey(g.Key) ? categories[g.Key].Name : "Unknown",
                Type = categories.ContainsKey(g.Key) ? categories[g.Key].Type : CategoryType.Both,
                TotalAmount = g.Sum(t => t.Amount),
                TransactionCount = g.Count()
            })
            .OrderByDescending(cs => cs.TotalAmount)
            .ToList();

        return categoryStats;
    }

    public List<MonthlyStatisticsDto> GetMonthly(int? year)
    {
        var transactions = _transactionRepository.GetAll();

        // Filter by year if provided
        if (year.HasValue)
        {
            transactions = transactions.Where(t => t.Date.Year == year.Value);
        }

        var monthlyStats = transactions
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new MonthlyStatisticsDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                TotalIncome = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                TotalExpenses = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
                Balance = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount) - 
                          g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
                TransactionCount = g.Count()
            })
            .OrderByDescending(m => m.Year)
            .ThenByDescending(m => m.Month)
            .ToList();

        return monthlyStats;
    }

    private static IEnumerable<Transaction> FilterByDateRange(
        IEnumerable<Transaction> transactions, 
        DateTime? startDate, 
        DateTime? endDate)
    {
        if (startDate.HasValue)
        {
            transactions = transactions.Where(t => t.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            transactions = transactions.Where(t => t.Date <= endDate.Value);
        }

        return transactions;
    }
}
