using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

public class MongoDBRepository<T> : IRepository<T> where T : IEntity
{
    #region Properties
    private readonly IMongoCollection<T> _Collection;
    public IMongoCollection<T> Collection => _Collection;
    #endregion

    #region Constructor
    public MongoDBRepository(IMongoDatabase database)
    {
        _Collection = database.GetCollection<T>(typeof(T).Name);
    }
    #endregion

    #region Methods
    public async Task<T?> GetByIdAsync(string id) =>
        await _Collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    public async Task<List<T>> GetAllAsync() =>
        await _Collection.Find(_ => true).ToListAsync();
    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _Collection.Find(predicate).ToListAsync();
    public async Task AddAsync(T entity) =>
        await _Collection.InsertOneAsync(entity);
    public async Task UpdateAsync(T entity) =>
        await _Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    public async Task DeleteAsync(string id) =>
        await _Collection.DeleteOneAsync(e => e.Id == id);
    #endregion
}
