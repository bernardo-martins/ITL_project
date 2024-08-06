using Anikatze.Application;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Anikatze.Application;

namespace Anikatze.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CourseService _service;

        public CoursesController(IMapper mapper, CourseService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("{courseGuid}")]
        public IActionResult GetCourses(string courseGuid)
        {
            return Ok(_service.GetCourseByCourseGuid(courseGuid));
        }

        [HttpPost]
        public IActionResult AddCourse(CourseDto courseDto)
        {
            try
            {
                var course = _mapper.Map<Course>(courseDto);
                _service.AddCourse(course);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}