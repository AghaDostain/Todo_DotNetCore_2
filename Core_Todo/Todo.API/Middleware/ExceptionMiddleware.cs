using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Todo.WebAPI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Todo.WebAPI
{
    public sealed class ExceptionMiddleware
    {
        public const string DefaultErrorMessage = "Error occurred!";
        private readonly IHostingEnvironment Enviroment;
        private readonly JsonSerializer Serializer;
        private readonly RequestDelegate Next;
        public ExceptionMiddleware(RequestDelegate next) => this.Next = next;
        public ExceptionMiddleware(IHostingEnvironment env)
        {
            Enviroment = env;
            Serializer = new JsonSerializer();
            Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context,ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context,Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var exc = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exc == null) return null;
            var error = BuildError(exc, Enviroment);
            var resultObject = JsonConvert.SerializeObject( new ContentActionResult<ExceptionDetail>(HttpStatusCode.InternalServerError, error, "INTERNAL SERVER ERROR", null));
            var contentActionResult = JsonConvert.DeserializeObject<ContentActionResult<ExceptionDetail>>(resultObject);
            var result = JsonConvert.SerializeObject(contentActionResult.ObjectResult.Value);
            return context.Response.WriteAsync(result);
        }
        private static ExceptionDetail BuildError(Exception ex, IHostingEnvironment env)
        {
            var error = new ExceptionDetail();
            if (env.IsDevelopment())
            {
                error.Message = ex.Message;
                error.Detail = ex.StackTrace;
            }
            else
            {
                error.Message = DefaultErrorMessage;
                error.Detail = ex.Message;
            }
            return error;
        }
    }    
}