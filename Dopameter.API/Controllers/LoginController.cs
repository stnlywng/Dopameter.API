using Dopameter.Common.DTOs;
using Dopameter.Repository;
using Dopameter.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dopameter.Controllers;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginRepository _loginRepository;
    private readonly IAuthService _authService;

    public LoginController(IConfiguration config, ILogger<LoginController> logger, ILoginRepository loginRepository, IAuthService authService)
    {
        _config = config;
        _logger = logger;
        _loginRepository = loginRepository;
        _authService = authService;
    }
    
    // Return UserID by (Username or Email) and Password
    [HttpPost]
    [Route("api/login")]
    public async Task<ActionResult<LoginSuccessResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("Called: " + nameof(Login));
        
        // See if user inputted an email instead of username
        if (loginRequest.username.Contains("@"))
        {
            var response = await _loginRepository.GetUserByEmail(loginRequest);
            if (response == null)
            {
                return NotFound(new { Message = "Login failed." });
            }
            
            var token = _authService.GenerateJwtToken(response.userID.ToString(), response.username);
            response.token = token;
            
            return Ok(response);
        }
        else
        {
            var response = await _loginRepository.GetUserByUsername(loginRequest);
            if (response == null)
            {
                return NotFound(new { Message = "Login failed." });
            }
            
            var token = _authService.GenerateJwtToken(response.userID.ToString(), response.username);
            response.token = token;

            return Ok(response);
        }
    }
    
    // Return UserID by (Username or Email) and Password
    [HttpPost]
    [Route("api/signup")]
    public async Task<ActionResult<LoginSuccessResponse>> SignUp([FromBody] SignUpRequest signUpRequest)
    {
        _logger.LogInformation("Called: " + nameof(SignUp));

        var response = await _loginRepository.CreateUser(signUpRequest);
        if (response == null)
        {
            return BadRequest(new { Message = "SignUp failed." });
        }
        
        var token = _authService.GenerateJwtToken(response.userID.ToString(), response.username);
        response.token = token;
        
        return Ok(response);
    }

    
}