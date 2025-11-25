using Microsoft.AspNetCore.Mvc;
using BudgetTracker.Application.DTOs;
using BudgetTracker.Application.Interfaces;

namespace BudgetTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public ActionResult<List<CategoryDto>> GetAll()
    {
        try
        {
            var categories = _categoryService.GetAll();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving categories.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryDto> GetById(int id)
    {
        try
        {
            var category = _categoryService.GetById(id);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found." });

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the category.", error = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<CategoryDto> Create([FromBody] CreateCategoryDto dto)
    {
        try
        {
            var category = _categoryService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the category.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult<CategoryDto> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        try
        {
            var category = _categoryService.Update(id, dto);
            if (category == null)
                return NotFound(new { message = $"Category with ID {id} not found." });

            return Ok(category);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the category.", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var result = _categoryService.Delete(id);
            if (!result)
                return NotFound(new { message = $"Category with ID {id} not found." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the category.", error = ex.Message });
        }
    }
}
