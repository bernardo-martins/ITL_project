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

    public class BlobsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly BlobService _service;

        public BlobsController(BlobService service)
        {
            _service = service;
        }
        
        [HttpGet("get-sas-token/{containerName}")]
        public IActionResult GetSasToken(string containerName)
        {
            var sasTokenUrl = _service.GenerateSasToken(containerName);
            return Ok(new { sasTokenUrl });
        }
    }
}