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
        public ExceptionMiddleware(IHostingEnvironment env)
        {
            Enviroment = env;
            Serializer = new JsonSerializer();
            Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;
            var error = BuildError(ex, Enviroment);
            using (var writer = new StreamWriter(context.Response.Body))
            {
                Serializer.Serialize(writer, error);
                await writer.FlushAsync().ConfigureAwait(false);
            }
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