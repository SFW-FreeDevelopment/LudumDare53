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
    [SwaggerResponse(StatusCodes.Status200OK, "Get ordered player list by name", typeof(List<PlayerDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    public async Task<IActionResult> Get()
    {
        var players = await _playerRepository.Get();
        return Ok(players.Select(PlayerResourceToDto));
    }

    [HttpGet("players/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get player resource by id", typeof(PlayerDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    [SwaggerResponse(StatusCodes.Status404NotFound, null)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var player = await _playerRepository.Get(id);
        return Ok(PlayerResourceToDto(player));
    }
    
    [HttpGet("players/getTopTen")]
    [SwaggerResponse(StatusCodes.Status200OK, "Get the current top 10 players", typeof(List<PlayerDto>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    public async Task<IActionResult> GetTopTen()
    {
        var players = await _playerRepository.GetTopTen();
        return Ok(players.Select(PlayerResourceToDto));
    }

    [HttpPost("processGameResults")]
    [SwaggerResponse(StatusCodes.Status201Created, "Called when a game has ended, creates player resource to be displayed on leaderboard", typeof(PlayerDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, null)]
    public async Task<IActionResult> ProcessGameResults([FromBody] PlayerDto request)
    {
        var requestJson = JsonSerializer.Serialize(request);
        _logger.LogTrace("Request: {requestJson}", requestJson);
        var player = await _playerRepository.Create(new Player(request));
        return Created($"/players/{player.Id}", PlayerResourceToDto(player));
    }

    private static PlayerDto PlayerResourceToDto(Player playerResource)
    {
        if (playerResource is null) return new PlayerDto {Name = ""};
        
        return new PlayerDto
        {
            Name = playerResource.Name,
            DaysCompleted = playerResource.DaysCompleted,
            TotalMoneyEarned = playerResource.TotalMoneyEarned,
            DeliveriesMade = playerResource.DeliveriesMade
        };
    }
}