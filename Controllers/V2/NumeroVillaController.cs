using AutoMapper;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MagicVilla_API.Controllers.V2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [ApiVersion("2.0")]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly INumeroVillaRepository _numerovillaRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepository villaRepo, INumeroVillaRepository numerovillaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numerovillaRepo = numerovillaRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valor1", "valor2" };
        }

    }
}
