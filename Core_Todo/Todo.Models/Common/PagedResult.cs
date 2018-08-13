using System.Collections.Generic;

namespace Todo.Models
{
    public class PagedResult<T> where T : class
    {
        public IList<T> Items { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
