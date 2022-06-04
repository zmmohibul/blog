using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<CreatePostDto, Post>();
            CreateMap<UpdatePostDto, Post>();
            CreateMap<Post, PostToReturnDto>().ForMember(destination => destination.CreatedBy, options => options.MapFrom(source => source.CreatedBy.Username));

            
        }  
    }
}