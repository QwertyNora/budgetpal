using BudgetTracker.Application.DTOs;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Core.Entities;
using BudgetTracker.Core.Interfaces;

namespace BudgetTracker.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Transaction> _transactionRepository;

    public CategoryService(
        IRepository<Category> categoryRepository,
        IRepository<Transaction> transactionRepository)
    {
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
    }

    public List<CategoryDto> GetAll()
    {
        return _categoryRepository.GetAll()
            .OrderBy(c => c.IsCustom)
            .ThenBy(c => c.Name)
            .Select(MapToDto)
            .ToList();
    }

    public CategoryDto? GetById(int id)
    {
        var category = _categoryRepository.GetById(id);
        return category == null ? null : MapToDto(category);
    }

    public CategoryDto Create(CreateCategoryDto dto)
    {
        // Validate unique name
        ValidateUniqueName(dto.Name);

        // Create entity
        var category = new Category
        {
            Name = dto.Name,
            Type = dto.Type,
            IsCustom = true,
            CreatedAt = DateTime.UtcNow
        };

        _categoryRepository.Add(category);
        _categoryRepository.SaveChanges();

        return MapToDto(category);
    }

    public CategoryDto? Update(int id, UpdateCategoryDto dto)
    {
        var category = _categoryRepository.GetById(id);
        if (category == null)
            return null;

        // Only custom categories can be updated
        if (!category.IsCustom)
            throw new InvalidOperationException("Cannot update predefined categories.");

        // Validate unique name (excluding current category)
        ValidateUniqueName(dto.Name, id);

        // Update entity
        category.Name = dto.Name;
        category.Type = dto.Type;

        _categoryRepository.Update(category);
        _categoryRepository.SaveChanges();

        return MapToDto(category);
    }

    public bool Delete(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category == null)
            return false;

        // Only custom categories can be deleted
        if (!category.IsCustom)
            throw new InvalidOperationException("Cannot delete predefined categories.");

        // Category cannot be deleted if it has transactions
        var hasTransactions = _transactionRepository.GetAll()
            .Any(t => t.CategoryId == id);

        if (hasTransactions)
            throw new InvalidOperationException("Cannot delete category with existing transactions.");

        _categoryRepository.Delete(category);
        _categoryRepository.SaveChanges();

        return true;
    }

    private void ValidateUniqueName(string name, int? excludeId = null)
    {
        var existingCategory = _categoryRepository.GetAll()
            .FirstOrDefault(c => c.Name.ToLower() == name.ToLower() && 
                                 (!excludeId.HasValue || c.Id != excludeId.Value));

        if (existingCategory != null)
            throw new ArgumentException($"Category with name '{name}' already exists.");
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type,
            IsCustom = category.IsCustom,
            CreatedAt = category.CreatedAt
        };
    }
}
