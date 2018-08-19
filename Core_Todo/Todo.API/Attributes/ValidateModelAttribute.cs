using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Todo.Common.Exceptions;
namespace Todo.WebAPI.Attributes
{
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys.SelectMany(key => context.ModelState[key].Errors.Select(error => new ValidationError(key,error.ErrorMessage))).ToList();
                context.Result = new ContentActionResult<IList<ValidationError>>(HttpStatusCode.BadRequest, errors, "BAD REQUEST", null);
            }
        }
    }
}