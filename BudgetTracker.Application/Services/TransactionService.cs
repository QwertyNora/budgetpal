using BudgetTracker.Application.DTOs;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Core.Entities;
using BudgetTracker.Core.Interfaces;

namespace BudgetTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Category> _categoryRepository;

    public TransactionService(
        IRepository<Transaction> transactionRepository,
        IRepository<Category> categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public PaginatedResponse<TransactionDto> GetAll(int pageNumber, int pageSize)
    {
        // Ensure valid pagination parameters
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        // Get all transactions ordered by Date DESC, then Id DESC
        var allTransactions = _transactionRepository.GetAll()
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.Id)
            .ToList();

        var totalCount = allTransactions.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Apply pagination
        var paginatedTransactions = allTransactions
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Map to DTOs
        var transactionDtos = paginatedTransactions.Select(MapToDto).ToList();

        return new PaginatedResponse<TransactionDto>
        {
            Data = transactionDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNext = pageNumber < totalPages,
            HasPrevious = pageNumber > 1
        };
    }

    public TransactionDto? GetById(int id)
    {
        var transaction = _transactionRepository.GetById(id);
        return transaction == null ? null : MapToDto(transaction);
    }

    public TransactionDto Create(CreateTransactionDto dto)
    {
        // Validate
        ValidateTransaction(dto.Date, dto.Description, dto.Amount, dto.CategoryId);

        // Create entity
        var transaction = new Transaction
        {
            Date = dto.Date,
            Description = dto.Description,
            Amount = dto.Amount,
            Type = dto.Type,
            CategoryId = dto.CategoryId,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _transactionRepository.Add(transaction);
        _transactionRepository.SaveChanges();

        return MapToDto(transaction);
    }

    public TransactionDto? Update(int id, UpdateTransactionDto dto)
    {
        var transaction = _transactionRepository.GetById(id);
        if (transaction == null)
            return null;

        // Validate
        ValidateTransaction(dto.Date, dto.Description, dto.Amount, dto.CategoryId);

        // Update entity
        transaction.Date = dto.Date;
        transaction.Description = dto.Description;
        transaction.Amount = dto.Amount;
        transaction.Type = dto.Type;
        transaction.CategoryId = dto.CategoryId;
        transaction.Notes = dto.Notes;
        transaction.UpdatedAt = DateTime.UtcNow;

        _transactionRepository.Update(transaction);
        _transactionRepository.SaveChanges();

        return MapToDto(transaction);
    }

    public bool Delete(int id)
    {
        var transaction = _transactionRepository.GetById(id);
        if (transaction == null)
            return false;

        _transactionRepository.Delete(transaction);
        _transactionRepository.SaveChanges();

        return true;
    }

    private void ValidateTransaction(DateTime date, string description, decimal amount, int categoryId)
    {
        // Date cannot be in future
        if (date > DateTime.UtcNow)
            throw new ArgumentException("Transaction date cannot be in the future.");

        // Description required
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.");

        // Amount must be > 0
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        // CategoryId must exist
        var category = _categoryRepository.GetById(categoryId);
        if (category == null)
            throw new ArgumentException($"Category with ID {categoryId} does not exist.");
    }

    private TransactionDto MapToDto(Transaction transaction)
    {
        var category = _categoryRepository.GetById(transaction.CategoryId);
        
        return new TransactionDto
        {
            Id = transaction.Id,
            Date = transaction.Date,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            CategoryName = category?.Name ?? "Unknown",
            Notes = transaction.Notes,
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }
}
