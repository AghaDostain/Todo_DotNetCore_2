using AutoMapper;
using Todo.Entities;
using Todo.Models;
using Todo.Repositories;

namespace Todo.Services
{
    public class UserTaskManager : ManagerBase<UserTaskModel, UserTask>, IUserTaskManager
    {
        private readonly IUserTaskRepository UserTaskRepository;
        public UserTaskManager(IUserTaskRepository repository, IMapper mapper) :base(repository, mapper)
        {
            UserTaskRepository = repository;
        }
    }
}