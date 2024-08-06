using Anikatze.Application;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Anikatze.Application;
using System.Reflection.Metadata.Ecma335;
using Anikatze.Application.Commands;

namespace Anikatze.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserQuizzesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserQuizService _service;

        public UserQuizzesController(IMapper mapper, UserQuizService service)
        {
            _mapper = mapper;
            _service = service;
        }



        [HttpPost("add-userquiz")]
        public IActionResult AddUserUserQuiz([FromBody] NewUserQuizCmd cmd)
        {
            if (cmd == null)
            {
                return BadRequest("Invalid input data");
            }
            try
            {
                var userQuizGuid = _service.AddUserQuizCompletion(cmd);
                return Ok(new { UserQuizGuid = userQuizGuid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-userquiz/{userGuid}")]
        public IActionResult GetUserQuizByUserId(string userGuid)
        {
            return Ok(_service.GetAllUserQuizzesByUserId(userGuid));
        }
        
        [HttpGet("check-userquiz/{userId}/{lectionId}")]
        public IActionResult CheckUserQuiz(int userId, int lectionId)
        {
            return Ok(_service.CheckUserQuizzesWithLectionQuiz(userId, lectionId));
        }
    }
}
