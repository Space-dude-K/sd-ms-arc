using AutoMapper;
using Entities.DTO.FileDto;
using Entities.DTO.FileDto.Update;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.DTO.ForumDto.Update;
using Entities.DTO.UserDto;
using Entities.DTO.UserDto.Create;
using Entities.DTO.UserDto.Update;
using Entities.Models;
using Entities.Models.File;
using Entities.Models.Forum;
using Entities.ViewModels;
using Entities.ViewModels.Forum;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Forum
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<AppUser, ForumUserDto>();
            CreateMap<UserForCreationDto, AppUser>();
            CreateMap<ForumUser, ForumUserForUpdateDto>();
            CreateMap<ForumUser, ForumUserForUpdateDto>().ReverseMap();
            CreateMap<ForumUser, ForumUserDto>();
            CreateMap<ForumUserDto, ForumUserPageViewModel>();

            // App user
            CreateMap<AppUser, AppUserForUpdateDto>();
            CreateMap<AppUser, AppUserForUpdateDto>().ReverseMap();
            CreateMap<ForumUserPageViewModel, AppUserDto>();

            // File
            CreateMap<ForumFileDto, ForumFile>();
            CreateMap<ForumFileDto, ForumFile>().ReverseMap();
            CreateMap<ForumFileForUpdateDto, ForumFile>();
            CreateMap<ForumFileForUpdateDto, ForumFile>().ReverseMap();

            // Category
            CreateMap<ForumCategory, ForumCategoryDto>();
            CreateMap<ForumCategoryForCreationDto, ForumCategory>();
            CreateMap<ForumCategoryForUpdateDto, ForumCategory>();
            CreateMap<ForumCategoryForUpdateDto, ForumCategory>().ReverseMap();

            // Forum
            CreateMap<ForumBase, ForumBaseDto>();
            CreateMap<ForumBaseForCreationDto, ForumBase>();
            CreateMap<ForumBaseForUpdateDto, ForumBase>();
            CreateMap<ForumBaseForUpdateDto, ForumBase>().ReverseMap();

            // Topic
            CreateMap<ForumTopic, ForumTopicDto>();
            CreateMap<ForumTopicForCreationDto, ForumTopic>();
            CreateMap<ForumTopicForUpdateDto, ForumTopic>();
            CreateMap<ForumTopicForUpdateDto, ForumTopic>().ReverseMap();

            // Topic counter
            CreateMap<ForumTopicCounter, ForumTopicCounterDto>();
            CreateMap<ForumTopicCounter, ForumTopicCounterForUpdateDto>();
            CreateMap<ForumTopicCounter, ForumTopicCounterForUpdateDto>().ReverseMap();
            CreateMap<ForumTopicCounter, ForumCounterForCreationDto>().ReverseMap();

            // Post
            CreateMap<ForumPost, ForumPostDto>();
            CreateMap<ForumPostForCreationDto, ForumPost>();
            CreateMap<ForumPostForUpdateDto, ForumPost>();
            CreateMap<ForumPostForUpdateDto, ForumPost>().ReverseMap();

            // Views
            CreateMap<RegisterViewModel, UserForCreationDto>();
            CreateMap<LoginViewModel, UserForAuthenticationDto>();

            CreateMap<ForumViewCategoryDto, SelectListItem>()
                .ForMember(dest => dest.Value, m => m.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, m => m.MapFrom(src => src.Name));
            CreateMap<ForumHomeViewModel, ForumBaseCreationView>()
                .ForMember(dest => dest.Categories, m => m.MapFrom(src => src.Categories));
            CreateMap<ForumBaseCreationView, ForumBaseForCreationDto>()
                .ForMember(dest => dest.ForumTitle, m => m.MapFrom(src => src.ForumTitle))
                .ForMember(dest => dest.ForumSubTitle, m => m.MapFrom(src => src.ForumSubtitle));

            CreateMap<ForumCategoryCreationView, ForumCategoryForCreationDto>()
                .ForMember(dest => dest.Name, m => m.MapFrom(src => src.CategoryName));

            CreateMap<ForumTopicCreationView, ForumTopicForCreationDto>()
                .ForMember(dest => dest.Name, m => m.MapFrom(src => src.TopicName));

            CreateMap<ForumTopicViewModel, ForumPostForCreationDto>()
                .ForMember(dest => dest.PostText, m => m.MapFrom(src => src.PostText))
                .ForMember(dest => dest.ForumTopicId, m => m.MapFrom(src => src.TopicId));
        }
    }
}