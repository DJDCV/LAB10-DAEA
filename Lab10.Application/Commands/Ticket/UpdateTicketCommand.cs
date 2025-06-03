using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Ticket;

public class UpdateTicketCommand : IRequest<bool>
{
    public ticket TicketToUpdate { get; set; } = null!;
}

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<ticket>();

        repo.Update(request.TicketToUpdate);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}