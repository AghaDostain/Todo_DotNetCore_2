using System.Net;
using System.Threading.Tasks;
using Todo.WebAPI.Extra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Todo.WebAPI
{
    public class ContentActionResult<T> : IActionResult where T : class
    {
        public ObjectResult objectResult { get; set; }
        public ContentActionResult(HttpStatusCode status, T data, string message, HttpRequest request, int total = 1)
        {
            objectResult = new ObjectResult(data)
            {
                StatusCode = (int)status,
                Value = ContentHelper.GetContent(status,message,data,request,total )
            };
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            await objectResult.ExecuteResultAsync(context);
        }
    }
}