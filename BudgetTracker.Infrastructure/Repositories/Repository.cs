using Microsoft.EntityFrameworkCore;
using BudgetTracker.Core.Interfaces;
using BudgetTracker.Infrastructure.Data;

namespace BudgetTracker.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BudgetTrackerDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(BudgetTrackerDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public int Count()
    {
        return _dbSet.Count();
    }
}
