using System.Collections.Generic;

namespace Todo.Entities
{
    public class PagedData<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}