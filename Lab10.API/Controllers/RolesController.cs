using Lab10.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("delcarpio/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleService _roleService;

    public RolesController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleRequest request)
    {
        var success = await _roleService.AssignRoleToUser(request.UserId, request.RoleId);
        if (!success)
            return BadRequest("El usuario ya tiene ese rol o datos inválidos.");

        return Ok("Rol asignado correctamente.");
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRoles(Guid userId)
    {
        var roles = await _roleService.GetRolesForUser(userId);
        return Ok(roles);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var success = await _roleService.CreateRoleAsync(roleName);
        if (!success)
            return BadRequest("El rol ya existe.");

        return Ok("Rol creado correctamente.");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    
}

public class AssignRoleRequest
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}