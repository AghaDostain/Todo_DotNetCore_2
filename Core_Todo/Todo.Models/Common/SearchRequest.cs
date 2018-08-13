using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Todo.Models
{
    public class SearchRequest : BaseRequest
    {
        public SearchRequest()
        {
            this.Filters = new List<FilterInfo>();
        }

        public IList<FilterInfo> Filters { get; set; }
    }
}
