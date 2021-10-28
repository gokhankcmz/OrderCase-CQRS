using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : Document
    {
        private readonly IMongoCollection<T> _collection;

        public Repository(IMongoClient client, MongoSettings settings)
        {
            var collectionName = typeof(T).Name;
            collectionName = collectionName[0].ToString().ToLowerInvariant() + collectionName[1..];
            _collection = client.GetDatabase(settings.DatabaseName).GetCollection<T>(collectionName);
        }

        #region AsyncMethods
        public async Task<T> CreateAsync(T document)
        {
            document.CreatedAt = DateTime.Now;
            document.UpdatedAt = DateTime.Now;
            await _collection.InsertOneAsync(document);
            return document;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var obj = await _collection.AsQueryable().FirstOrDefaultAsync(x => x.Id.Equals(id));
            if(obj != null ) obj.ReadFrom = "Mongo";
            return obj;
        }

        public async Task<T> ReplaceAsync(T document)
        {
            await _collection.ReplaceOneAsync(x => x.Id.Equals(document.Id), document);
            return document;
        }

        public DeleteResult Delete(T document)
        {
            return _collection.DeleteOne(x => x.Id.Equals(document.Id));
        }

        public async Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _collection.AsQueryable().Where(expression).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.AsQueryable().ToListAsync();
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> expression)
        {
            await _collection.DeleteManyAsync(expression);
        }

        #endregion


        #region SyncMethods
        public List<T> GetAll()
        {
            return _collection.AsQueryable().ToList();
        }

        public List<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            
            return _collection.AsQueryable().Where(expression).ToList();
        }
        
        public  T Create(T document)
        {
            document.CreatedAt = DateTime.Now;
            document.UpdatedAt = DateTime.Now;
            _collection.InsertOne(document);
            return document;
        }

        public T GetById(Guid id)
        {
            return _collection.AsQueryable().Where(x => x.Id.Equals(id)).First();
        }

        public T Replace(T document)
        {
              _collection.ReplaceOne(x => x.Id.Equals(document.Id), document);
            return document;
        }
        #endregion
    }
}