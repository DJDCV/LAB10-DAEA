using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Queries.Ticket;

public class GetAllTicketsQuery : IRequest<IEnumerable<ticket>>
{
}

public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<ticket>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTicketsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ticket>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<ticket>();
        return await repo.GetAllAsync();
    }
}