using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Todo.Entities;
using Todo.Common;

namespace Todo.Repositories
{
    public abstract class GenericRepository<TEntity> : IDisposable, IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext context;

        public GenericRepository(DbContext context)
        {
            this.context = context;
        }

        //public GenericRepository()
        //{
        //    this.context = new PortalDataContext();
        //}
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await SaveAsync();
            return entity;
        }

        public virtual void Attach(TEntity entity)
        {
            context.Set<TEntity>().Attach(entity);
        }

        public virtual async Task<int> CountAsync()
        {
            return await context.Set<TEntity>().CountAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                Attach(entity);
            }
            context.Set<TEntity>().Remove(entity);
            await SaveAsync();
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entity = await context.Set<TEntity>().FindAsync(id);
            if (entity != null)
                await DeleteAsync(entity);
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public virtual async Task<PagedData<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, string orderBy = null, int? pageNumber = 0, int? pageSize = 0, string includeProperties = "")
        {
            //out int totalCount,
            IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
            var result = new PagedData<TEntity>();
            try
            {
                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
                //Make sure property to order on is a valid property of the object, otherwise user default
                //Get distinct rows
                query = query.Distinct();
                //Get the count
                //result.TotalCount = await query.CountAsync();
                result.Items = await Task.FromResult(OrderedPage(query, orderBy, pageNumber, pageSize).ToList());
                result.TotalCount = result.Items.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual async Task<PagedData<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, string orderBy = null, int? pageNumber = 0, int? pageSize = 0, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            //out int totalCount,
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeExpressions.Any())
            {
                foreach (var includeExpression in includeExpressions)
                {
                    query = query.Include(includeExpression);
                }
            }
            var result = new PagedData<TEntity>();
            try
            {
                //Make sure property to order on is a valid property of the object, otherwise user default
                //Get distinct rows
                query = query.Distinct();
                //Get the count
                //result.TotalCount = await query.CountAsync();
                result.Items = await Task.FromResult(OrderedPage(query, orderBy, pageNumber, pageSize).ToList());
                result.TotalCount = result.Items.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<PagedData<TEntity>> GetAllAsync(string orderBy = null, int? pageNumber = 0, int? pageSize = 0)
        {
            var result = new PagedData<TEntity>();
            //Make sure property to order on is a valid property of the object, otherwise user default
            //Get distinct rows
            var query = context.Set<TEntity>().AsQueryable();
            query = query.Distinct();
            //Get the count
            //result.TotalCount = await query.CountAsync();
            result.Items = await Task.FromResult(OrderedPage(query, orderBy, pageNumber, pageSize).ToList());
            result.TotalCount = result.Items.Count();
            return result;
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = this.context.Set<TEntity>();
            query = query.Where(filter);
            if (includeExpressions.Any())
            {
                foreach (var includeExpression in includeExpressions)
                {
                    query = query.Include(includeExpression);
                }
            }
            return await query.SingleOrDefaultAsync<TEntity>();
        }

        public virtual async Task SaveAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException duex)
            {
                //ExceptionHelper.Process(duex);
                throw (duex);
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, Func<TEntity, bool> key = null)
        {
            InternalUpdate(entity, key);
            await SaveAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> filter,
            IEnumerable<object> updatedSet, // Updated many-to-many relationships
            string propertyName) // The name of the navigation property
        {
            // Get the generic type of the set
            var type = updatedSet.GetType().GetGenericArguments()[0];
            // Get the previous entity from the database based on repository type
            var previous = await context
                .Set<TEntity>()
                .Include(propertyName)
                .FirstOrDefaultAsync(filter);
            /* Create a container that will hold the values of
                * the generic many-to-many relationships we are updating.
                */
            var values = CreateList(type);
            /* For each object in the updated set find the existing
                 * entity in the database. This is to avoid Entity Framework
                 * from creating new objects or throwing an
                 * error because the object is already attached.
                 */
            //TODO: Move key field name to configuration
            foreach (var entry in updatedSet
                .Select(obj => (long)obj
                    .GetType()
                    .GetProperty("Id")
                    .GetValue(obj, null))
                .Select(value => context.Set<TEntity>().Find(value)))//(type).Find(value)))
            {
                values.Add(entry);
            }
            /* Get the collection where the previous many to many relationships
                * are stored and assign the new ones.
                */
            context.Entry(previous).Collection(propertyName).CurrentValue = values;
        }

        public virtual async Task UpdateAsync<TRelated>(Expression<Func<TEntity, bool>> filter, IEnumerable<TRelated> relatedItems, Expression<Func<TEntity, IEnumerable<TRelated>>> includeExpression)
            where TRelated : class
        {
            // Get the generic type of the set
            //var type = relatedItems.GetType().GetGenericArguments()[0];
            // Get the previous entity from the database based on repository type
            var previous = await context.Set<TEntity>()
                                        .Include(includeExpression)
                                        .FirstOrDefaultAsync(filter);
            /* Create a container that will hold the values of
                * the generic many-to-many relationships we are updating.
                */
            var relatedValues = new List<TRelated>();
            //CreateList(type);
            /* For each object in the updated set find the existing
                 * entity in the database. This is to avoid Entity Framework
                 * from creating new objects or throwing an
                 * error because the object is already attached.
                 */
            //TODO: Move key field name to configuration
            //foreach (var entry in relatedItems.Select(obj => (long)obj.GetType()
            //                                                        .GetProperty("Id")
            //                                                        .GetValue(obj, null))
            //                                .Select(value => context.Set(type).Find(value)))
            //{
            //    values.Add(entry);
            //}
            foreach (var item in relatedItems)
            {
                var id = item.GetType().GetProperty("Id").GetValue(item, null);
                var entry = await context.Set<TRelated>().FindAsync(id);
                relatedValues.Add(entry);
            }
            /* Get the collection where the previous many to many relationships
                * are stored and assign the new ones.
                */
            context.Entry(previous).Collection(includeExpression).CurrentValue.DefaultIfEmpty();
            //foreach (var item in relatedValues)
            //{
            context.Entry(previous).Collection(includeExpression).CurrentValue.Concat(relatedValues);
            //}
        }

        protected void InternalUpdate(TEntity entity, Func<TEntity, bool> key)
        {
            var entry = context.Entry<TEntity>(entity);
            if (entry.State == EntityState.Detached)
            {
                var set = context.Set<TEntity>();
                TEntity attachedEntity = null == key ? null : set.Local.SingleOrDefault(key);
                if (attachedEntity != null)
                {
                    var attachedEntry = context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }
        }

        protected IQueryable<TEntity> OrderedPage(IQueryable<TEntity> query, string orderBy = null, int? pageNumber = 0, int? pageSize = 0)
        {
            orderBy = GuardOrderBy(orderBy);
            query = query.OrderBy<TEntity>(orderBy);
            //Changed . Min page number is 1.
            if (pageSize != null && pageNumber != null && pageSize > 0 && pageNumber >= 1)
            {
                query = query.Skip((int)(pageNumber - 1) * (int)pageSize).Take((int)pageSize);
            }
            return query;
        }

        private IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }

        private string GuardOrderBy(string orderBy)
        {
            //TODO: Move default order to configuration
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = orderBy.Trim();
                var index = orderBy.IndexOf(" ");
                if (index > 0)
                {
                    var orderByField = orderBy.Substring(0, index + 1).Trim();
                    var orderByOrder = orderBy.Substring(index, orderBy.Length - index).Trim();
                    orderByField = TypeHelper.MapToValidPropertyName<TEntity>(orderByField);
                    //if either of these are invalid default order by id
                    if (orderByField == null ||
                        !(orderByOrder.Equals("asc", StringComparison.InvariantCultureIgnoreCase)
                        || orderByOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return "Id desc";
                    }
                    else
                        return string.Concat(orderByField, " ", orderByOrder);
                }
                else
                {
                    var orderByField = TypeHelper.MapToValidPropertyName<TEntity>(orderBy);
                    if (orderByField == null)
                        return "Id desc";
                    else
                        return orderByField;
                }
            }
            else
                return "Id desc";
        }
    }
}