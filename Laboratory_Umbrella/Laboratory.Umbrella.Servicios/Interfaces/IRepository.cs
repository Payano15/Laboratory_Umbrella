using Laboratory.Umbrella.Dominio.Entities;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface IRepository<T> where T : IEntity
{
    Task<T?> GetByIdAsync(string id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
    IMongoCollection<T> Collection { get; }
}
