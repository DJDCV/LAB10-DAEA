using Lab10.Application.DTOs;
using Lab10.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
    {
        var response = await _authService.LoginAsync(loginDto);

        if (response == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(response);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserDto userDto)
    {
        var result = await _authService.RegisterAsync(userDto);

        if (!result)
        {
            return BadRequest("User already exists or invalid data");
        }

        return Ok("User registered successfully");
    }
}