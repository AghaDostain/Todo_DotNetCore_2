using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Models
{
    public class KeyedModel<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}
