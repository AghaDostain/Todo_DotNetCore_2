namespace Todo.Models
{
    public class BaseRequest
    {
        protected string sort;
        protected int pageSize;
        protected int page;
        //private string filter;

        public string Sort
        {
            get
            {
                var result = "Id";
                var definition = new { field = "", dir = "" };
                if (string.IsNullOrEmpty(this.sort))
                    return result;
                else {
                    return this.sort;
                }
            }
            set
            {
                sort = value;
            }
        }
        public int PageSize
        {
            get
            {
                return this.pageSize <= 0 ? 0 : this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
        public int Page
        {
            get
            {
                return this.page < 0 ? 0 : this.page;
            }
            set
            {
                this.page = value;
            }
        }

        //public FilterCollection Filters
        //{
        //    get
        //    {
        //        var filters = filter == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<FilterCollection>(filter);
        //        return filters;
        //    }
        //}


        //public string Filter
        //{
        //    set
        //    {
        //        this.filter = value;
        //    }
        //    get
        //    {
        //        return filter;
        //    }
        //}
    }
}
