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
    public class UserLectionCompletionsController : ControllerBase
    {
       private readonly IMapper _mapper;
       private readonly UserLectionCompletionService _service;

        public UserLectionCompletionsController(IMapper mapper, UserLectionCompletionService service)
        {
              _mapper = mapper;
              _service = service;
        }

        [HttpGet]
        public IActionResult GetAllCompletions()
        {
            return Ok(_service.GetAllCompletions());
        }

        [HttpGet("{userGuid}")]
        public IActionResult GetUserCompletions(string userGuid)
        {
            return Ok(_service.GetAllCompletionsByUserId(userGuid));
        }

        [HttpGet("last/{userGuid}")]
        public IActionResult GetUserCompletion(string userGuid)
        {
            return Ok(_service.LastThreeCompletions(userGuid));
        }
        [HttpPost("add-completion")]
        public IActionResult AddUserLectionCompletion([FromBody] NewUserLectionCompletionCmd cmd)
        {
            if (cmd == null)
            {
                return BadRequest("Invalid input data");
            }
            
            try
            {
                var completionGuid = _service.AddCompletion(cmd);
                return Ok(new { CompletionGuid = completionGuid });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
        [HttpGet("time/{id}")]
        public IActionResult GetTimePerMonth(int id)
        {
            return Ok(_service.GetCompletionsTime(id));
        }

        [HttpGet("completed/{userGuid}/{lectionGuid}")]
        public IActionResult GetCompletedLections(string userGuid, string lectionGuid)
        {
            return Ok(_service.IsCompleted(userGuid, lectionGuid));
        }
    }
}
