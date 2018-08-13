using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Todo.WebAPI.Extra
{
    public class ContentHelper
    {
        //public static ObjectContent<object> GetContent(HttpRequestMessage request, System.Net.HttpStatusCode code, string message, object data)
        //{
        //    return new ObjectContent<object>(new
        //    {
        //        status = code,
        //        message = message,
        //        data = data,
        //        requestId = System.Diagnostics.Trace.CorrelationManager.ActivityId
        //    }, request.GetConfiguration().Formatters.JsonFormatter);
        //}
        internal static object GetContent<T>(HttpStatusCode status, string message, T data, HttpRequest request, int total) where T : class
        {
            return new
            {
                status = status,
                message = message,
                data = data,
                total = total,
                requestId = System.Diagnostics.Trace.CorrelationManager.ActivityId
            };
        }
        //public static IActionResult InvalidRequest(HttpRequestMessage request, string message)
        //{
        //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
        //    {
        //        Content = ContentHelper.GetContent(request, System.Net.HttpStatusCode.BadRequest, message, "")
        //    });
        //}
        //public static IActionResult InvalidRequest(HttpRequestMessage request, ModelStateDictionary ModelState)
        //{
        //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
        //    {
        //        Content = ContentHelper.GetContent(request, System.Net.HttpStatusCode.BadRequest, "An error occurred", ModelState)
        //    });
        //}
    }
}
