using BudgetTracker.Application.DTOs;

namespace BudgetTracker.Application.Interfaces;

public interface IStatisticsService
{
    StatisticsDto GetOverallStatistics(DateTime? startDate, DateTime? endDate);
    List<CategoryStatisticsDto> GetByCategory(DateTime? startDate, DateTime? endDate);
    List<MonthlyStatisticsDto> GetMonthly(int? year);
}
