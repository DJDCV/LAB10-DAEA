using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Queries.Response;

public class GetResponsesByTicketIdQuery : IRequest<IEnumerable<response>>
{
    public Guid TicketId { get; set; }
}

public class GetResponsesByTicketIdQueryHandler : IRequestHandler<GetResponsesByTicketIdQuery, IEnumerable<response>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetResponsesByTicketIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<response>> Handle(GetResponsesByTicketIdQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<response>();
        var allResponses = await repo.GetAllAsync();

        return allResponses.Where(r => r.ticket_id == request.TicketId);
    }
}