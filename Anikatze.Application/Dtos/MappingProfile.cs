using Anikatze.Application.Commands;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using AutoMapper;

namespace Anikatze.Application.Dtos
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            CreateMap<User, AllUsersDto>();
            CreateMap<Lection, LectionsDto>();
            CreateMap<Lection, LectionDto>();
            CreateMap<User, OnlyUserDto>();
            CreateMap<UserLectionCompletion, UserLectionCompletionsDto>();
            CreateMap<Quiz, QuizDto>();
            CreateMap<NewUserLectionCompletionCmd, UserLectionCompletion>();
            CreateMap<NewUserQuizCmd, UserQuiz>();
            CreateMap<UserQuiz, UserQuizDto>();
            CreateMap<Course, CourseDto>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();
        }
    }
}