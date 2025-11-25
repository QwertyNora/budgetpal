using BudgetTracker.Application.DTOs;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionService
{
    PaginatedResponse<TransactionDto> GetAll(int pageNumber, int pageSize);
    TransactionDto? GetById(int id);
    TransactionDto Create(CreateTransactionDto dto);
    TransactionDto? Update(int id, UpdateTransactionDto dto);
    bool Delete(int id);
}
