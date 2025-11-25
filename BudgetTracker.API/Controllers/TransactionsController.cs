using Microsoft.AspNetCore.Mvc;
using BudgetTracker.Application.DTOs;
using BudgetTracker.Application.Interfaces;

namespace BudgetTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public ActionResult<PaginatedResponse<TransactionDto>> GetAll(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var result = _transactionService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving transactions.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<TransactionDto> GetById(int id)
    {
        try
        {
            var transaction = _transactionService.GetById(id);
            if (transaction == null)
                return NotFound(new { message = $"Transaction with ID {id} not found." });

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the transaction.", error = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<TransactionDto> Create([FromBody] CreateTransactionDto dto)
    {
        try
        {
            var transaction = _transactionService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the transaction.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult<TransactionDto> Update(int id, [FromBody] UpdateTransactionDto dto)
    {
        try
        {
            var transaction = _transactionService.Update(id, dto);
            if (transaction == null)
                return NotFound(new { message = $"Transaction with ID {id} not found." });

            return Ok(transaction);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the transaction.", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var result = _transactionService.Delete(id);
            if (!result)
                return NotFound(new { message = $"Transaction with ID {id} not found." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the transaction.", error = ex.Message });
        }
    }
}
