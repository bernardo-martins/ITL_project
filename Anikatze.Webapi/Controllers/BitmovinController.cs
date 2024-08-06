using Anikatze.Application;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Anikatze.Application;
using Microsoft.Extensions.Logging;

namespace Anikatze.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BitmovinController : ControllerBase
    {
        private readonly BitmovinService _service;
        private readonly ILogger<BitmovinController> _logger;

        public BitmovinController(BitmovinService service,ILogger<BitmovinController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("encode")]
        public async Task<IActionResult> Encode()
        {
            try
            {
                _logger.LogInformation("Starting encoding process.");
                await _service.CreateAzureInput();
                await _service.CreateAzureOutput();
                await _service.CreateEncoding();
                await _service.CreateEncodingHLS();
                await _service.CreateStreams();
                await _service.CreateMuxings();
                await _service.CreateHlsManifest();
                await _service.StartEncoding();
                _logger.LogInformation("Encoding process completed.");
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}