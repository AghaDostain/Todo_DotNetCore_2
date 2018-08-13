using System.Threading.Tasks;
using Todo.Models;
using Todo.WebAPI;
using Todo.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Todo.Services;
using Todo.WebAPI.Attributes;

namespace TodoWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserTaskController : BaseController<UserTaskModel>
    {
        private readonly IUserTaskManager UserTaskModelManager;
        public UserTaskController(IUserTaskManager domainManager) : base(domainManager)
        {
            UserTaskModelManager = domainManager;
        }
        [ProducesResponseType(200,  Type = typeof(ContentActionResult<UserTaskModel>))]
        [HttpPost]
        [ValidateModel]
        public override Task<IActionResult> Create([FromBody] UserTaskModel model)
        {
            return base.Create(model);
        }
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
        [ProducesResponseType(200, Type = typeof(ContentActionResult<UserTaskModel>))]
        [HttpGet("{id:int}")]
        public override Task<IActionResult> Get(int id)
        {
            return base.Get(id);
        }
        [ProducesResponseType(200, Type = typeof(ContentActionResult<UserTaskModel>))]
        [HttpGet]
        public override Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            return base.GetAll(request);
        }
        [ProducesResponseType(200, Type = typeof(ContentActionResult<UserTaskModel>))]
        [HttpPost("search")]
        public override Task<IActionResult> Search(SearchRequest request)
        {
            return base.Search(request);
        }
        [ProducesResponseType(200, Type = typeof(ContentActionResult<UserTaskModel>))]
        [HttpPut("{id:int}")]
        [ValidateModel]
        public override Task<IActionResult> Update([FromBody] UserTaskModel model)
        {
            return base.Update(model);
        }
    }
}