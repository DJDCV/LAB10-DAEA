using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Queries.Role;

public class GetRolesForUserQuery : IRequest<List<string>>
{
    public Guid UserId { get; set; }
}

public class GetRolesForUserQueryHandler : IRequestHandler<GetRolesForUserQuery, List<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRolesForUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<string>> Handle(GetRolesForUserQuery request, CancellationToken cancellationToken)
    {
        var userRoleRepo = _unitOfWork.Repository<user_role>();
        var userRoles = await userRoleRepo.GetAllAsync();

        return userRoles
            .Where(ur => ur.user_id == request.UserId)
            .Select(ur => ur.role.role_name)
            .ToList();
    }
}