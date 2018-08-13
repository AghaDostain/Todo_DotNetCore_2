using System.Threading.Tasks;
using Todo.Models;
namespace Todo.Services
{
    public interface IManager<TModel> where TModel: class
    {
        Task<TModel> GetAsync(int id);
        Task<PagedResult<TModel>> GetAsync(PagingRequest request);
        Task<TModel> AddAsync(TModel user);
        Task<TModel> UpdateAsync(TModel user);
        Task DeleteAsync(TModel user);
        Task DeleteAsync(int id);
        Task<PagedResult<TModel>> SearchAsync(SearchRequest request, string includeProperties = "");
    }
}