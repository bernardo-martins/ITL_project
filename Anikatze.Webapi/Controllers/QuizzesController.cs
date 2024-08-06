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

    public class QuizzesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly QuizService _service;

        public QuizzesController(IMapper mapper, QuizService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("{lectionGuid}")]
        public IActionResult GetQuizzes(string lectionGuid)
        {
            return Ok(_service.GetQuizzesByLectionGuid(lectionGuid));
        }
        [HttpPost("check")]
        public IActionResult CheckUserAnswers([FromBody] CheckUserAnswersRequest request)
        {
            var result = _service.CheckUserAnswers(request.QuizGuid, request.UserAnswers);
            return Ok(new { success = result });
        }
        
        public class CheckUserAnswersRequest
        {
            public string QuizGuid { get; set; }
            public Dictionary<string, string> UserAnswers { get; set; }
        }
    } 
}