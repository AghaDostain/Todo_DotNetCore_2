using Todo.Common.Enumerations;
namespace Todo.Models
{
    public class FilterInfo
    {
        public string Field { get; set; }
        public FilterOperators Op { get; set; }
        public object Value { get; set; }
    }
}