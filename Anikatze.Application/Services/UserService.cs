using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using Anikatze.Application.Dtos;

namespace Anikatze.Application.Services
{
    public class UserService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;
        public UserService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        
        public IQueryable<User> Users => _db.Set<User>().AsQueryable();
        public IQueryable<UserCourse> UserCourse => _db.Set<UserCourse>().AsQueryable();
        public IQueryable<Course> Courses => _db.Set<Course>().AsQueryable();

        public IEnumerable GetAllUsers()
        {
            var users = _mapper.ProjectTo<AllUsersDto>(Users)
                .ToList();
            return users;
        }
        

        public IEnumerable<UserCourseDto> GetUserCourses(string userGuid)
        {
            var userCourses = Users
                .Join(UserCourse, user => user.UserID, userCourse => userCourse.UserID,
                    (user, userCourse) => new { user, userCourse })
                .Join(Courses, combined => combined.userCourse.CourseID, course => course.CourseID,
                    (combined, course) => new { combined.user, combined.userCourse, course })
                .Where(u => u.user.UserGuid == userGuid)
                .Select(u => new UserCourseDto(u.user.UserID, u.user.Username, u.course.CourseGuid, u.course.Description))
                .ToList();
            return userCourses;
        }

        public OnlyUserDto GetUser(string userGuid)
        {
            var user = Users.Where(u => u.UserGuid == userGuid);
            return _mapper.ProjectTo<OnlyUserDto>(user).FirstOrDefault();
        }
    }
}