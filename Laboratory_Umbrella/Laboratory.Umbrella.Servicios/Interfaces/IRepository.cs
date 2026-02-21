using Laboratory.Umbrella.Dominio.Entities;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    IQueryable<T> Query();
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);

    Task<bool> ExistsAsync(string id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    Task<List<T>> FindWithIncludesAsync(
    Expression<Func<T, bool>> predicate,
    params Expression<Func<T, object>>[] includes);
    Task<(List<T> Items, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    Expression<Func<T, bool>>? filter = null,
    Expression<Func<T, object>>? orderBy = null,
    bool ascending = true);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task DeleteRangeAsync(IEnumerable<string> ids);

    Task<int> SaveChangesAsync();
}