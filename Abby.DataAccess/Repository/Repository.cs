using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Abby.DataAccess.Repository;

public class Repository<T> : IRepository.IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }
    
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, 
                                 Func<IQueryable<T>,IOrderedQueryable<T>>? orderby = null, 
                                 string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(",",StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderby != null)
        {
            return orderby(query).ToList();
        }
        
        return query.ToList();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(",",StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        
        return query.FirstOrDefault()!;
    }
}