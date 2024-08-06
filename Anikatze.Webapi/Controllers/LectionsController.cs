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

    public class LectionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly LectionService _service;
        
        public LectionsController(IMapper mapper, LectionService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("{courseGuid}")] // gibt dir ein Array von {LectionID, Title und VideoID}-Objekten zur�ck -- funktioniert nach DB-Umstellung
        public IActionResult GetLections(string courseGuid)
        {
            return Ok(_service.GetLectionsByCourseGuid(courseGuid));
        }
        [HttpGet("lection/{lectionGuid}")] // gibt dir LectionID, Title und VideoID zur�ck -- funktioniert nach DB-Umstellung
        public IActionResult GetLection(string lectionGuid)
        {
            return Ok(_service.GetLectionByLectionGuid(lectionGuid));
        }
        
    }
}