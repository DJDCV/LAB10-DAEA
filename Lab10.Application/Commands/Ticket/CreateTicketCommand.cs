using MediatR;
using Lab10.Application.DTOs;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Ticket;

public class CreateTicketCommand : IRequest<bool>
{
    public CreateTicketDto Ticket { get; set; } = null!;
}

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<ticket>();

        var newTicket = new ticket
        {
            ticket_id = Guid.NewGuid(),
            user_id = request.Ticket.UserId,
            title = request.Ticket.Title,
            description = request.Ticket.Description,
            status = request.Ticket.Status,
            created_at = DateTime.UtcNow
        };

        await repo.AddAsync(newTicket);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}