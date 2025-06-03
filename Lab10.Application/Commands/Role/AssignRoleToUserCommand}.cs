using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Role;

public class AssignRoleToUserCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoleToUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var userRoleRepo = _unitOfWork.Repository<user_role>();

        var existing = (await userRoleRepo.GetAllAsync())
            .FirstOrDefault(ur => ur.user_id == request.UserId && ur.role_id == request.RoleId);

        if (existing != null)
            return false;

        var userRole = new user_role
        {
            user_id = request.UserId,
            role_id = request.RoleId,
            assigned_at = DateTime.UtcNow
        };

        await userRoleRepo.AddAsync(userRole);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}