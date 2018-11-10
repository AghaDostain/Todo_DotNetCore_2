using Microsoft.AspNetCore.Identity;

namespace Todo.Entities
{
    public partial class Role : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
