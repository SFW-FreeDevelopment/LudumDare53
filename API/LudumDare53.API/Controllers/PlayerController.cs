using System.Text.Json;
using LudumDare53.API.Database.Repositories;
using LudumDare53.API.Models;
using LudumDare53.API.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LudumDare53.API.Controllers;

[ApiController]
[Route("players")]
public class PlayerController : ControllerBase
{
    private readonly PlayerRepository _playerRepository;
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger, PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
            _logger = logger;
        }
        
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(List<Player>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _playerRepository.Get());
        }
        
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(Player))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
        [SwaggerResponse(StatusCodes.Status404NotFound, null)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            return Ok(await _playerRepository.Get(id));
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, null, typeof(Player))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
        public async Task<IActionResult> Create([FromBody] PlayerCreateRequest request)
        {
            var player = await _playerRepository.Create(new Player(request));
            return Created($"player/{player.Id}", player);
        }
        
        [HttpPatch("{id}/processGameResults")]
        [SwaggerResponse(StatusCodes.Status200OK, null, typeof(Player))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
        public async Task<IActionResult> ProcessGameResults([FromRoute] string id, [FromBody] ProcessGameResultsRequest request)
        {
            var requestJson = JsonSerializer.Serialize(request);
            _logger.LogTrace("Id: {id}\nRequest: {requestJson}", id, requestJson);
            var player = await _playerRepository.Get(id);
            UpdatePlayerStats(player, request);
            return Ok(await _playerRepository.Update(id, player));
        }

        private static void UpdatePlayerStats(Player player, ProcessGameResultsRequest request)
        {
            // if (request.Waves > player.Waves)
            // {
            //     player.Waves = request.Waves;
            // }
            //
            // if (request.Score > player.Score)
            // {
            //     player.Score = request.Score;
            // }
        }
}