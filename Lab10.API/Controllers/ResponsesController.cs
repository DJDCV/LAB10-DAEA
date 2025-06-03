using Lab10.Application.Commands.Response;
using Lab10.Application.DTOs;
using Lab10.Application.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("delcarpio/[controller]")]
[ApiController]
[Authorize]
public class ResponsesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResponsesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ticket/{ticketId}")]
    public async Task<IActionResult> GetResponsesByTicket(Guid ticketId)
    {
        var query = new GetResponsesByTicketIdQuery { TicketId = ticketId };
        var responses = await _mediator.Send(query);
        return Ok(responses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResponse([FromBody] CreateResponseDto dto)
    {
        var command = new CreateResponseCommand { Response = dto };
        var success = await _mediator.Send(command);
        if (!success) return BadRequest("No se pudo crear la respuesta");
        return Ok("Respuesta creada correctamente");
    }
}