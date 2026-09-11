using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Data.Context;

namespace StaffManagement.Data.Base;

/// <summary>Repository base theo pattern của SeniorEssentials.</summary>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected StaffManagementContext _context;

    public GenericRepository(StaffManagementContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public void PrepareCreate(T entity) => _context.Add(entity);
    public void PrepareUpdate(T entity) => _context.Entry(entity).State = EntityState.Modified;
    public void PrepareRemove(T entity) => _context.Remove(entity);
    public int Save() => _context.SaveChanges();
    public Task<int> SaveAsync() => _context.SaveChangesAsync();
    public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression).AsNoTracking();
    public Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression).AsNoTracking().ToListAsync();
    public List<T> GetAll() => _context.Set<T>().ToList();
    public Task<List<T>> GetAllAsync() => _context.Set<T>().ToListAsync();
    public T? GetById(int id) => _context.Set<T>().Find(id);
    public Task<T?> GetByIdAsync(int id) => _context.Set<T>().FindAsync(id).AsTask();

    public Task<int> CreateAsync(T entity)
    {
        _context.Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task<int> UpdateAsync(T entity)
    {
        PrepareUpdate(entity);
        return _context.SaveChangesAsync();
    }

    public async Task<bool> RemoveAsync(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
