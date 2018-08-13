using System;
using Todo.Exceptions;

namespace Todo.Common.Exceptions
{
    [Serializable]
    public class DuplicateRecordException : BaseException
    {
        public DuplicateRecordException() : base("Duplicate data is found") { }
        public DuplicateRecordException(string message) : base(message) { }
        public DuplicateRecordException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateRecordException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
