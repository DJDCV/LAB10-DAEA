using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Queries.Ticket;

public class GetTicketByIdQuery : IRequest<ticket?>
{
    public Guid TicketId { get; set; }
}

public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, ticket?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ticket?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<ticket>();
        return await repo.GetByIdAsync(request.TicketId);
    }
}