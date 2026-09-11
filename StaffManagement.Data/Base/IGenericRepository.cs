using System.Linq.Expressions;

namespace StaffManagement.Data.Base;

/// <summary>Contract chung để repository typed vẫn expose các hàm CRUD/search.</summary>
public interface IGenericRepository<T> where T : class
{
    void PrepareCreate(T entity);
    void PrepareUpdate(T entity);
    void PrepareRemove(T entity);
    int Save();
    Task<int> SaveAsync();
    IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
    List<T> GetAll();
    Task<List<T>> GetAllAsync();
    T? GetById(int id);
    Task<T?> GetByIdAsync(int id);
    Task<int> CreateAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<bool> RemoveAsync(T entity);
}
