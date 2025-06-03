using Lab10.Application.Commands.Role;
using Lab10.Application.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("delcarpio/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequest request)
    {
        var command = new AssignRoleToUserCommand
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        var success = await _mediator.Send(command);
        if (!success)
            return BadRequest("El usuario ya tiene ese rol o los datos son inválidos.");

        return Ok("Rol asignado correctamente.");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRoles(Guid userId)
    {
        var query = new GetRolesForUserQuery { UserId = userId };
        var roles = await _mediator.Send(query);
        return Ok(roles);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var command = new CreateRoleCommand { RoleName = roleName };
        var success = await _mediator.Send(command);

        if (!success)
            return BadRequest("El rol ya existe.");

        return Ok("Rol creado correctamente.");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoles()
    {
        var query = new GetAllRolesQuery();
        var roles = await _mediator.Send(query);
        return Ok(roles);
    }
}

public class AssignRoleRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}