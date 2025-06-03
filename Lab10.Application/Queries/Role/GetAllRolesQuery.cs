using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Queries.Role;

public class GetAllRolesQuery : IRequest<IEnumerable<role>>
{
}

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<role>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllRolesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<role>();
        return await repo.GetAllAsync();
    }
}