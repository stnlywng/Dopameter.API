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
    private readonly IGremlinRepository _gremlinRepository;
    private readonly IAuthService _authService;

    public LoginController(IConfiguration config, ILogger<LoginController> logger, ILoginRepository loginRepository, IAuthService authService, IGremlinRepository gremlinRepository)
    {
        _config = config;
        _logger = logger;
        _loginRepository = loginRepository;
        _authService = authService;
        _gremlinRepository = gremlinRepository;
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
    
    // On Front-End Side it'll Ignore the generated UserID for DEMO and just use -5.
    [HttpPost]
    [Route("api/login/demo")]
    public async Task<ActionResult<LoginSuccessResponse>> LoginDemo()
    {
        _logger.LogInformation("Called: " + nameof(LoginDemo));

        var loginRequest = new LoginRequest { username = "demo", password = "demo" };
        var signUpRequest = new SignUpRequest { username = "demo", password = "demo", email = "demo"};
        
        // Try and Login, if DNE throws error and returns null, so there you handle the Sign-Up
        var loginResponse = await _loginRepository.GetUserByUsername(loginRequest);
        if (loginResponse != null)
        {
            await _gremlinRepository.SetupDemoGremlins();
            var token = _authService.GenerateJwtToken("-5", loginResponse.username);
            loginResponse.token = token;
            loginResponse.userID = -5;

            return Ok(loginResponse);
        }
        else
        {
            var signUpResponse = await _loginRepository.CreateUser(signUpRequest);
            if (signUpResponse == null)
            {
                return BadRequest(new { Message = "Demo Account Login/SignUp failed." });
            }
            
            await _gremlinRepository.SetupDemoGremlins();
            var token = _authService.GenerateJwtToken("-5", signUpResponse.username);
            signUpResponse.token = token;
            signUpResponse.userID = -5;
            
            return Ok(signUpResponse);
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