using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using Anikatze.Application.Dtos;
using static System.Collections.Specialized.BitVector32;

namespace Anikatze.Application.Services
{
    public class CourseService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;

        public CourseService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        
        public IQueryable<Course> Courses => _db.Set<Course>().AsQueryable();

        public IEnumerable GetCourses(string courseGuid)
        {
            var course = Courses.FirstOrDefault(c => c.CourseGuid == courseGuid);

            if (course == null)
            {
                throw new Exception("Course not found");
            }
            var courseId = course.CourseID;

            var courses = _mapper.ProjectTo<CourseDto>(Courses
                                   .Where(c => c.CourseGuid == courseGuid))
                .ToList();
            return courses;
        }

        public CourseDto GetCourseByCourseGuid(string courseGuid)
        {
            var courseDto = Courses
                .Where(c => c.CourseGuid == courseGuid)
                .Select(c => new CourseDto(c.CourseID, c.Name,c.Price))
                .FirstOrDefault();
            return courseDto;
        }

        public void AddCourse(Course course)
        {
            _db.Add(course);
            _db.SaveChanges();
        }
    }
}