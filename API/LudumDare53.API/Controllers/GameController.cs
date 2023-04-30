using System.Text.Json;
using LudumDare53.API.Database.Repositories;
using LudumDare53.API.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace LudumDare53.API.Controllers;

[ApiController]
public class GameController : ControllerBase
{
    private readonly PlayerRepository _playerRepository;
    private readonly ILogger<GameController> _logger;

    public GameController(ILogger<GameController> logger, PlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    [HttpGet("players")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get ordered player list by name", typeof(List<Player>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    public async Task<IActionResult> Get()
    {
        return Ok(await _playerRepository.Get());
    }

    [HttpGet("players/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get player resource by id", typeof(Player))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    [SwaggerResponse(StatusCodes.Status404NotFound, null)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        return Ok(await _playerRepository.Get(id));
    }

    [HttpPost("processGameResults")]
    [SwaggerResponse(StatusCodes.Status201Created, "Called when a game has ended, creates player resource to be displayed on leaderboard", typeof(Player))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    public async Task<IActionResult> ProcessGameResults([FromBody] Player request)
    {
        var requestJson = JsonSerializer.Serialize(request);
        _logger.LogTrace("Request: {requestJson}", requestJson);
        var player = await _playerRepository.Create(request);
        return Created($"/players/{player.Id}", player);
    }
}