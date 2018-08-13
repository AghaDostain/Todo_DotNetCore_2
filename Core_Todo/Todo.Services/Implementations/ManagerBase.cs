using AutoMapper;
using Todo.Entities;
using Todo.Models;
using Todo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Todo.Services
{
    public abstract class ManagerBase<TModel, TEntity> : IManager<TModel>
        where TModel : class //Model
        where TEntity : class //Entity
    {
        protected readonly IMapper Mapper;
        protected readonly IGenericRepository<TEntity> Repository;

        public ManagerBase(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            this.Repository = repository;
            this.Mapper = mapper;
        }

        public virtual async Task<TModel> AddAsync(TModel model)
        {
            var result = await Repository.AddAsync(this.Mapper.Map<TEntity>(model));
            return this.Mapper.Map<TModel>(result);
        }

        public virtual async Task DeleteAsync(TModel model)
        {
            await Repository.DeleteAsync(this.Mapper.Map<TEntity>(model));
        }

        public virtual async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }

        public virtual async Task<TModel> GetAsync(int id)
        {
            var result = await Repository.GetByIdAsync(id);
            return this.Mapper.Map<TModel>(result);
        }

        public virtual async Task<PagedResult<TModel>> GetAsync(PagingRequest request)
        {
            request = request != null ? request : new PagingRequest();
            PagedData<TEntity> data = await GetInternalAsync(request);
            return GetPagedResult(request, data);
        }

        protected virtual PagedResult<TModel> GetPagedResult(PagingRequest request, PagedData<TEntity> data)
        {
            var result = new PagedResult<TModel>();
            result.Items = this.Mapper.Map<List<TModel>>(data.Items);
            result.Page = request.Page;
            result.PageSize = request.PageSize;
            result.TotalCount = data.TotalCount;
            return result;
        }

        protected virtual async Task<PagedData<TEntity>> GetInternalAsync(PagingRequest request)
        {
            //Build the filter expression
            //var lambda = request.Filters == null ? null : request.Filters.BuildFiltersLambda<TEntity>();
            Expression<Func<TEntity, bool>> lambda = null;
            var data = await Repository.FindAsync(lambda, request.Sort, request.Page, request.PageSize);
            return data;
        }

        public virtual async Task<PagedResult<TModel>> SearchAsync(SearchRequest request, string includeProperties = "")
        {
            var result = new PagedResult<TModel>();
            var lambda = BuildFilters(request.Filters);
            var data = await Repository.FindAsync(lambda, request.Sort, request.Page, request.PageSize, includeProperties);
            result.Items = this.Mapper.Map<List<TModel>>(data.Items);
            result.Page = request.Page;
            result.PageSize = request.PageSize;
            result.TotalCount = data.TotalCount;
            return result;
        }

        public virtual Expression<Func<TEntity, bool>> BuildFilters(IList<FilterInfo> filters)
        {
            if (filters != null)
            {
                return filters.BuildFiltersLambda<TEntity>();
            }
            return null;
        }

        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            //Need to match the key of the entity (which is Id) with the key of the model (which is also id)
            //This is to handle the scenario when the entity is detatched but is already in the context.
            //Working with generic here use GetProperty
            //TODO: Id fieldname in config
            var result = await Repository.UpdateAsync(this.Mapper.Map<TEntity>(model), key => (int)key.GetType().GetProperty("Id").GetValue(key) == (int)model.GetType().GetProperty("Id").GetValue(model));
            return this.Mapper.Map<TModel>(result);
        }
    }
}