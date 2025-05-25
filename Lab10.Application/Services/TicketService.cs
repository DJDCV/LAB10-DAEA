using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;
using Lab10.Application.DTOs;

namespace Lab10.Application.Services;

public class TicketService
{
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ticket>> GetAllTicketsAsync()
    {
        var repo = _unitOfWork.Repository<ticket>();
        return await repo.GetAllAsync();
    }

    public async Task<ticket?> GetTicketByIdAsync(Guid ticketId)
    {
        var repo = _unitOfWork.Repository<ticket>();
        return await repo.GetByIdAsync(ticketId);
    }
    
    public async Task<bool> CreateTicketAsync(CreateTicketDto dto)
    {
        var repo = _unitOfWork.Repository<ticket>();

        var newTicket = new ticket
        {
            ticket_id = Guid.NewGuid(),
            user_id = dto.UserId,
            title = dto.Title,
            description = dto.Description,
            status = dto.Status,
            created_at = DateTime.UtcNow
        };

        await repo.AddAsync(newTicket);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }

    public async Task<bool> UpdateTicketAsync(ticket updatedTicket)
    {
        var repo = _unitOfWork.Repository<ticket>();
        repo.Update(updatedTicket);
        var result = await _unitOfWork.CommitAsync();
        return result > 0;
    }

    public async Task<bool> DeleteTicketAsync(Guid ticketId)
    {
        var repo = _unitOfWork.Repository<ticket>();
        var ticket = await repo.GetByIdAsync(ticketId);
        if (ticket == null) return false;
        repo.Delete(ticket);
        var result = await _unitOfWork.CommitAsync();
        return result > 0;
    }
}

public class ResponseService
{
    private readonly IUnitOfWork _unitOfWork;

    public ResponseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<response>> GetResponsesByTicketIdAsync(Guid ticketId)
    {
        var repo = _unitOfWork.Repository<response>();
        var allResponses = await repo.GetAllAsync();
        return allResponses.Where(r => r.ticket_id == ticketId);
    }

    // Modificado para usar DTO CreateResponseDto
    public async Task<bool> CreateResponseAsync(CreateResponseDto dto)
    {
        var repo = _unitOfWork.Repository<response>();

        var newResponse = new response
        {
            response_id = Guid.NewGuid(),
            ticket_id = dto.TicketId,
            responder_id = dto.ResponderId,
            message = dto.Message,
            created_at = dto.CreatedAt ?? DateTime.UtcNow
        };

        await repo.AddAsync(newResponse);
        var result = await _unitOfWork.CommitAsync();
        return result > 0;
    }
}
