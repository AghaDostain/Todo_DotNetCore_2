using Microsoft.AspNetCore.Mvc;
using Todo.Models;
using Todo.Services;
using Todo.WebAPI.Attributes;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
namespace Todo.WebAPI.Controllers
{
    [Route("api")]
    public abstract class BaseController<T> : Controller where T : class //DomainModel
    {
        protected readonly IManager<T> DomainManager;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domainManager">Domain manager concrete instance</param>
        public BaseController(IManager<T> domainManager)
        {
            this.DomainManager = domainManager;
        }
        /// <summary>
        /// GET All
        /// Signature: api/entity
        /// </summary>
        /// <param name="request">Paging and sorting model</param>
        /// <returns>IActionResult encapsulating IList of entities requested </returns>
        public virtual async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var result = await this.DomainManager.GetAsync(request);
            return new ContentActionResult<IList<T>>(HttpStatusCode.OK, result.Items, "OK", Request, result.TotalCount);
        }
        /// <summary>
        /// GET:
        /// Signature: api/entity/5
        /// </summary>
        /// <param name="id">Primary key of the entitty</param>
        /// <returns>IActionResult encapsulating the entity</returns>
        public virtual async Task<IActionResult> Get(int id)
        {
            var result = await this.DomainManager.GetAsync(id);
            return new ContentActionResult<T>(HttpStatusCode.OK, result, "OK", Request);
        }
        /// <summary>
        /// Create entity
        /// Signature: api/entity
        /// </summary>
        /// <param name="model">Entity domain model</param>
        /// <returns>IActionResult encapsulating Entity domain model mapped from created entity</returns>
        
        public virtual async Task<IActionResult> Create([FromBody] T model)
        {
            var result = await DomainManager.AddAsync(model);
            return new ContentActionResult<T>(HttpStatusCode.OK, result, "OK", Request);
        }
        /// <summary>
        /// Update entity
        /// Signature: api/entity
        /// </summary>
        /// <param name="model">Entity to be updated</param>
        /// <returns>IActionResult encapsulating Entity domain model mapped from created entity</returns>
        public virtual async Task<IActionResult> Update([FromBody]T model)
        {
            var result = await DomainManager.UpdateAsync(model);
            return new ContentActionResult<T>(HttpStatusCode.OK, result, "OK", Request);
        }
        /// <summary>
        /// Delete entity
        /// Signature: api/entity/5
        /// </summary>
        /// <param name="id">Primary key of the entity to be deleted</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200, Type = typeof(ContentActionResult<string>))]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await DomainManager.DeleteAsync(id);
            return new ContentActionResult<string>(HttpStatusCode.OK, string.Empty, "OK", Request);
        }
        // search: api/entity/search
        [HttpGet("search")]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Search(SearchRequest request)
        {
            var result = await DomainManager.SearchAsync(request);
            return new ContentActionResult<IList<T>>(HttpStatusCode.OK, result.Items, "OK", Request, result.TotalCount);
        }
    }
}