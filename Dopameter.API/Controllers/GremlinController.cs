using System.Security.Claims;
using Dopameter.BusinessLogic;
using Dopameter.Common.DTOs;
using Dopameter.Common.Models;
using Dopameter.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;

namespace Dopameter.Controllers;

[ApiController]
[Route("api/gremlin")]
public class GremlinController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IGremlinRepository _gremlinRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly ILogger<GremlinController> _logger;

    public GremlinController(IConfiguration config, IGremlinRepository gremlinRepository, IActivityRepository activityRepository, ILogger<GremlinController> logger)
    {
        _config = config;
        _gremlinRepository = gremlinRepository;
        _activityRepository = activityRepository;
        _logger = logger;
    }
    
    // Returning a Gremlin by ID
    [Authorize]
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
    [Authorize]
    [HttpGet("currentgremlins")]
    public async Task<ActionResult<IEnumerable<Gremlin>>> GetGremlinsByUser()
    {
        // Take the userId given through the JWT, and then change it to an integer and assign it to userId.
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
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
    [Authorize]
    [HttpGet("oldgremlins")]
    public async Task<ActionResult<IEnumerable<Gremlin>>> GetOldGremlinsByUser()
    {
        // Take the userId given through the JWT, and then change it to an integer and assign it to userId.
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        _logger.LogInformation("Called: " + nameof(GetOldGremlinsByUser));
        try
        {
            var result = await _gremlinRepository.GetOldGremlinsByUser(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetOldGremlinsByUser)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Old Gremlins for user with ID {userId} were not found, or Error" });
        }
    }
    
    // Deleting a Gremlin by ID -- TO-DO delete activity with gremlin if no other using it. [DONE?]
    // TO-DO only allow if the userID whatever is validated. Authentication. [Nahh it's good enough]
    [Authorize]
    [HttpDelete("{gremlinId}")]
    public async Task<ActionResult> DeleteGremlin(int gremlinId)
    {
        // Take the userId given through the JWT, and then change it to an integer and assign it to userId.
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        _logger.LogInformation("Called: " + nameof(DeleteGremlin));
        try
        {
            var gremlin = await _gremlinRepository.GetGremlinById(gremlinId);
            var gremlinActivityName = gremlin.activityName;
            
            await _gremlinRepository.DeleteGremlin(gremlinId);
            await _activityRepository.DeleteActivityIfNotAssociatedWithAnyMoreGremlins(userId, gremlinActivityName);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(DeleteGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Couldn't find Gremlin with ID {gremlinId}, or Error" });
        }
    }
    
    // Updating a Gremlin TO-DO update activity with gremlin
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<Gremlin>> UpdateGremlin([FromBody] Gremlin gremlin)
    {
        // Take the userId given through the JWT, and then change it to an integer and assign it to userId.
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        _logger.LogInformation("Called: " + nameof(UpdateGremlin));
        try
        {
            var result = await _gremlinRepository.UpdateGremlin(gremlin);
            await _activityRepository.CreateOrUpdatePastActivity(userId, gremlin.activityName, gremlin.kindOfGremlin, gremlin.intensity);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(UpdateGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Error with UpdateGremlin." });
        }
    }
    
    // Creating a Gremlin TO-DO add Activity too.
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateGremlin([FromBody] Gremlin gremlin)
    {
        // Take the userId given through the JWT, and then change it to an integer and assign it to userId.
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        gremlin.dateOfBirth = DateTime.Now;
        gremlin.lastFedDate = DateTime.Now;
        
        _logger.LogInformation("Called: " + nameof(CreateGremlin));
        try
        {
            await _gremlinRepository.CreateGremlin(userId, gremlin);
            await _activityRepository.CreateOrUpdatePastActivity(userId, gremlin.activityName, gremlin.kindOfGremlin, gremlin.intensity);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(CreateGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return NotFound(new { Message = $"Error with CreateGremlin" });
        }
    }
    
    // For testing. Not used in the app. Archive
    // [Authorize]
    // [HttpGet("feedhistory/{gremlinId}")]
    // public async Task<ActionResult<IEnumerable<FeedHistoryRow>>> GetGremlinFeedHistory(int gremlinId)
    // {
    //     _logger.LogInformation("Called: " + nameof(GetGremlinWeight));
    //     try
    //     {
    //         var result = await _gremlinRepository.GetGremlinFeedingHistory(gremlinId);
    //         return Ok(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError($"Error in {nameof(GetGremlinWeight)} : {ex.Message} with stack trace: {ex.StackTrace}");
    //         return NotFound(new { Message = $"Gremlin with ID {gremlinId} was not found." });
    //     }
    // }
    
    [Authorize]
    [HttpPost("feed")]
    public async Task<ActionResult> FeedGremlin([FromBody] FeedGremlinRequest _feedGremlinRequest)
    {
        _logger.LogInformation("Called: " + nameof(FeedGremlin));
        var gremlinId = _feedGremlinRequest.gremlinID;
        var percentFed = _feedGremlinRequest.percentFed;
        try
        {
            var gremlinInCheck = await _gremlinRepository.GetGremlinById(gremlinId);
            // (int gremlinId, int oldLastSetWeight, DateTime lastFedDate, int percentFed)
            await _gremlinRepository.FeedGremlin(gremlinId, gremlinInCheck.lastSetWeight, gremlinInCheck.lastFedDate, percentFed);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(FeedGremlin)} : {ex.Message} with stack trace: {ex.StackTrace}");
            return BadRequest(new { Message = $"Error with FeedGremlin" });
        }
    }
    
}