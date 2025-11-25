using Microsoft.AspNetCore.Mvc;
using BudgetTracker.Application.DTOs;
using BudgetTracker.Application.Interfaces;

namespace BudgetTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public ActionResult<StatisticsDto> GetOverallStatistics(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var statistics = _statisticsService.GetOverallStatistics(startDate, endDate);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving statistics.", error = ex.Message });
        }
    }

    [HttpGet("by-category")]
    public ActionResult<List<CategoryStatisticsDto>> GetByCategory(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var statistics = _statisticsService.GetByCategory(startDate, endDate);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving category statistics.", error = ex.Message });
        }
    }

    [HttpGet("monthly")]
    public ActionResult<List<MonthlyStatisticsDto>> GetMonthly([FromQuery] int? year = null)
    {
        try
        {
            var statistics = _statisticsService.GetMonthly(year);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving monthly statistics.", error = ex.Message });
        }
    }
}
