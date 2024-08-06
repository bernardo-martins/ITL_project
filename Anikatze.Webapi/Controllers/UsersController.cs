using Anikatze.Application;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Anikatze.Application;

namespace AspShowcase.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserService _service;
        
        public UsersController(IMapper mapper, UserService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllUsers() // Gibt dir ein Array von AllUsersDto zur�ck -- funktioniert nach DB-Umstellung
        {
            return Ok(_service.GetAllUsers());
        }

        [HttpGet("{userGuid}")]
        public IActionResult GetUserCourses(string userGuid) // Gibt dir ein Array von UserCourseDto zur�ck -- funktioniert nach DB-Umstellung
        {
            return Ok(_service.GetUserCourses(userGuid));
        }

        [HttpGet("user/{userGuid}")] // Gibt dir Username, Email und UserID zur�ck -- funktioniert nach DB-Umstellung 
        public IActionResult GetUser(string userGuid)
        {
            return Ok(_service.GetUser(userGuid));
        }
    }
}