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
    public class LectionService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;
        public LectionService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        
        public IQueryable<Lection> Lections => _db.Set<Lection>().AsQueryable();
        public IQueryable<Course> Courses => _db.Set<Course>().AsQueryable();
        
        public IEnumerable GetLectionsByCourseGuid(string courseGuid)
        {
            var course = Courses.FirstOrDefault(c => c.CourseGuid == courseGuid);
            
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            var courseId = course.CourseID;

            var lections = _mapper.ProjectTo<LectionsDto>(Lections
                    .Where(l => l.CourseID == courseId))
                .ToList();
            return lections;
        }
        public LectionDto GetLectionByLectionGuid(string LectionGuid)
        {
            var lectionDto = Lections
                    .Where(l => l.LectionGuid == LectionGuid)
                    .Select(l => new LectionDto(
                        l.LectionGuid,
                        l.Title,
                        l.Videos.FirstOrDefault() != null ? l.Videos.FirstOrDefault().VideoID.ToString() : null,
                        l.Text))
                    .FirstOrDefault();
            return lectionDto;
        }
    }
}