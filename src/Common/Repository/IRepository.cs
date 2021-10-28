using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities.Models;
using MongoDB.Driver;

namespace Repository
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T document);
        Task<T> GetByIdAsync(Guid id);
        Task<T> ReplaceAsync(T document);
        DeleteResult Delete(T document);

        Task DeleteManyAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAllAsync();


        List<T> GetAll();
        List<T> GetByCondition(Expression<Func<T, bool>> expression);
        T Create(T document);
        T GetById(Guid id);
        T Replace(T document);
    }
}