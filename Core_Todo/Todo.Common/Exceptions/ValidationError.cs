using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Common.Exceptions
{
    public class ValidationError
    {
        public ValidationError()
        {

        }
        public ValidationError(string field, string message)
        {
            Field = string.IsNullOrWhiteSpace(field) ? null : field;
            Message = message;
        }
        public string Field { get; set; }
        public string Message { get; set; }
    }
}