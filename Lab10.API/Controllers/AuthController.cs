using Lab10.Application.Commands.Auth;
using Lab10.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("delcarpio/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
    {
        var command = new LoginCommand
        {
            Email = loginDto.Email,
            Password = loginDto.Password
        };

        var response = await _mediator.Send(command);

        if (response == null)
            return Unauthorized("Credenciales inválidas");

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserDto userDto)
    {
        var command = new RegisterUserCommand
        {
            User = userDto
        };

        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest("El usuario ya existe");

        return Ok("Usuario registrado con éxito");
    }
}