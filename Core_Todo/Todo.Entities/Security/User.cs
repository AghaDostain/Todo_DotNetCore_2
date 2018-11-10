using System;
using Microsoft.AspNetCore.Identity;

namespace Todo.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLogin { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int RoleId { get; set; }
    }
}
