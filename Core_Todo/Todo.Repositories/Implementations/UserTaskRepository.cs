using Microsoft.EntityFrameworkCore;
using Todo.Entities;

namespace Todo.Repositories
{
    public class UserTaskRepository : GenericRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(DbContext context) : base(context)
        {

        }
    }
}
