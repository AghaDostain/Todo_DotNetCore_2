using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Todo.WebAPI
{
    public class ContentActionResult<T> : IActionResult where T : class
    {
        public ObjectResult ObjectResult { get; set; }
        public ContentActionResult(HttpStatusCode status, T data, string message, HttpRequest request, int total = 1) => ObjectResult = new ObjectResult(data)
        {
            StatusCode = (int)status,
            Value = new
            {
                status = status,
                message = message,
                data = data,
                total = total,
                requestId = System.Diagnostics.Trace.CorrelationManager.ActivityId
            }//ContentHelper.GetContent(status,message,data,request,total )
        };
        public async Task ExecuteResultAsync(ActionContext context) => await ObjectResult.ExecuteResultAsync(context);
    }
}