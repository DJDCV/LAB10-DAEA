using Lab10.Application.Commands.Ticket;
using Lab10.Application.DTOs;
using Lab10.Application.Queries.Ticket;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("delcarpio/[controller]")]
[ApiController]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = await _mediator.Send(new GetAllTicketsQuery());
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(Guid id)
    {
        var ticket = await _mediator.Send(new GetTicketByIdQuery { TicketId = id });
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
    {
        var command = new CreateTicketCommand { Ticket = dto };
        var success = await _mediator.Send(command);
        if (!success) return BadRequest("No se pudo crear el ticket");
        return Ok("Ticket creado correctamente");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var command = new DeleteTicketCommand { TicketId = id };
        var success = await _mediator.Send(command);
        if (!success) return BadRequest("No se pudo eliminar el ticket");
        return Ok("Ticket eliminado");
    }
}