using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Todo.Entities;
namespace Todo.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        void Attach(TEntity entity);
        Task<int> CountAsync();
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task<PagedData<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, string orderBy = null, int? pageNumber = 0, int? pageSize = 0, string includeProperties = "");
        Task<PagedData<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, string orderBy = null, int? pageNumber = 0, int? pageSize = 0, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<PagedData<TEntity>> GetAllAsync(string orderBy = null, int? pageNumber = 0, int? pageSize = 0);
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeExpressions);
        Task SaveAsync();
        Task<TEntity> UpdateAsync(TEntity entity, Func<TEntity, bool> key = null);
        Task UpdateAsync(Expression<Func<TEntity, bool>> filter,
            // Updated many-to-many relationships
            IEnumerable<object> updatedSet,
            // The name of the navigation property
            string propertyName);
        Task UpdateAsync<TRelated>(Expression<Func<TEntity, bool>> filter,
            // Updated many-to-many relationships
            IEnumerable<TRelated> updatedSet,
            //IEnumerable<object> availableSet
            Expression<Func<TEntity, IEnumerable<TRelated>>> includeExpression) where TRelated : class;
    }
}
