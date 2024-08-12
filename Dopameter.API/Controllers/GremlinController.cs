using Dopameter.Common.Models;
using Dopameter.Repository;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;

namespace Dopameter.Controllers;

[ApiController]
[Route("api/gremlin")]
public class GremlinController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IGremlinRepository _gremlinRepository;
    private readonly ILogger<GremlinController> _logger;

    public GremlinController(IConfiguration config, IGremlinRepository gremlinRepository, ILogger<GremlinController> logger)
    {
        _config = config;
        _gremlinRepository = gremlinRepository;
        _logger = logger;
    }
    
    // Returning a Gremlin by ID
    [HttpGet("{gremlinId}")]
    public async Task<ActionResult<Gremlin>> GetGremlinById(int gremlinId)
    {
        _logger.LogInformation("Called: " + nameof(GetGremlinById));
        try
        {
            var result = await _gremlinRepository.GetGremlinById(gremlinId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetGremlinById)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Gremlin with ID {gremlinId} was not found." });
        }
    }
    
    // Returning all Gremlins for a user
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Gremlin>>> GetGremlinsByUser(int userId)
    {
        _logger.LogInformation("Called: " + nameof(GetGremlinsByUser));
        try
        {
            var result = await _gremlinRepository.GetCurrentGremlinsByUser(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetGremlinsByUser)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Gremlins for user with ID {userId} were not found." });
        }
    }
    
    // Returning all Gremlins for a user
    [HttpGet("old/user/{userId}")]
    public async Task<ActionResult<IEnumerable<Gremlin>>> GetOldGremlinsByUser(int userId)
    {
        _logger.LogInformation("Called: " + nameof(GetOldGremlinsByUser));
        try
        {
            var result = await _gremlinRepository.GetOldGremlinsByUser(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetOldGremlinsByUser)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Old Gremlins for user with ID {userId} were not found." });
        }
    }
    
    // Deleting a Gremlin by ID
    [HttpDelete("{gremlinId}")]
    public async Task<ActionResult> DeleteGremlin(int gremlinId)
    {
        _logger.LogInformation("Called: " + nameof(DeleteGremlin));
        try
        {
            await _gremlinRepository.DeleteGremlin(gremlinId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(DeleteGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Gremlin with ID {gremlinId} was not found." });
        }
    }
    
    // Updating a Gremlin
    [HttpPut]
    public async Task<ActionResult<Gremlin>> UpdateGremlin(Gremlin gremlin)
    {
        _logger.LogInformation("Called: " + nameof(UpdateGremlin));
        try
        {
            var result = await _gremlinRepository.UpdateGremlin(gremlin);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(UpdateGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Gremlin with ID {gremlin.gremlinID} was not found." });
        }
    }
    
    // Creating a Gremlin
    [HttpPost("{userId}")]
    public async Task<ActionResult> CreateGremlin(int userId, Gremlin gremlin)
    {
        _logger.LogInformation("Called: " + nameof(CreateGremlin));
        try
        {
            await _gremlinRepository.CreateGremlin(userId, gremlin);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(CreateGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Gremlin with ID {gremlin.gremlinID} was not found." });
        }
    }
    
}