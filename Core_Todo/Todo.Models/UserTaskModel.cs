using System;

namespace Todo.Models
{
    public class UserTaskModel : KeyedModel<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
