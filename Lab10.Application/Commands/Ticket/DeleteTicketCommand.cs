using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Ticket;

public class DeleteTicketCommand : IRequest<bool>
{
    public Guid TicketId { get; set; }
}

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<ticket>();
        var ticket = await repo.GetByIdAsync(request.TicketId);

        if (ticket == null) return false;

        repo.Delete(ticket);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}