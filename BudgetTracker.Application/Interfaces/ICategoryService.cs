using BudgetTracker.Application.DTOs;

namespace BudgetTracker.Application.Interfaces;

public interface ICategoryService
{
    List<CategoryDto> GetAll();
    CategoryDto? GetById(int id);
    CategoryDto Create(CreateCategoryDto dto);
    CategoryDto? Update(int id, UpdateCategoryDto dto);
    bool Delete(int id);
}
