using AutoMapper;
using Todo.Entities;
using Todo.Models;

namespace Todo.Mappers.Profiles
{
    public class UserTaskProfile : Profile
    {
        public UserTaskProfile()
        {
            CreateMap<UserTask, UserTaskModel>().ReverseMap();
            //CreateMap<UserTaskModel, UserTask>();
        }
    }
}
