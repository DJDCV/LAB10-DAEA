using Lab10.Application.DTOs;
using Lab10.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab10_DelCarpioDeivid.Controllers;

[Route("delcarpio/[controller]")]
[ApiController]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(Guid id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null) return NotFound();
        return Ok(ticket);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
    {
        var success = await _ticketService.CreateTicketAsync(dto);
        if (!success) return BadRequest("No se pudo crear el ticket");
        return Ok("Ticket creado correctamente");
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var success = await _ticketService.DeleteTicketAsync(id);
        if (!success) return BadRequest("No se pudo eliminar el ticket");
        return Ok("Ticket eliminado");
    }
}

[Route("delcarpio/[controller]")]
[ApiController]
[Authorize]
public class ResponsesController : ControllerBase
{
    private readonly ResponseService _responseService;

    public ResponsesController(ResponseService responseService)
    {
        _responseService = responseService;
    }

    [HttpGet("ticket/{ticketId}")]
    public async Task<IActionResult> GetResponsesByTicket(Guid ticketId)
    {
        var responses = await _responseService.GetResponsesByTicketIdAsync(ticketId);
        return Ok(responses);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResponse([FromBody] CreateResponseDto dto)
    {
        var success = await _responseService.CreateResponseAsync(dto);
        if (!success) return BadRequest("No se pudo crear la respuesta");
        return Ok("Respuesta creada correctamente");
    }
}
