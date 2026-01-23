using Laboratory.Umbrella.Dominio.Entities;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Interfaces;

// ============================================
// IRepository Interface - Versión completa
// ============================================
public interface IRepository<T> where T : class, IEntity  // ← Asegúrate de tener "class" aquí
{
    // Operaciones básicas CRUD
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);

    // Operaciones adicionales útiles
    Task<bool> ExistsAsync(string id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    // Para consultas más complejas con includes y paginación
    Task<List<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes);

    Task<(List<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true);

    // Operaciones en lote
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task DeleteRangeAsync(IEnumerable<string> ids);

    // Para transacciones
    Task<int> SaveChangesAsync();
}