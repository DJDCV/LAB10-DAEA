using MediatR;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Role;

public class CreateRoleCommand : IRequest<bool>
{
    public string RoleName { get; set; } = null!;
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleRepo = _unitOfWork.Repository<role>();

        var existingRole = (await roleRepo.GetAllAsync())
            .FirstOrDefault(r => r.role_name.Equals(request.RoleName, StringComparison.OrdinalIgnoreCase));

        if (existingRole != null)
            return false;

        var role = new role
        {
            role_id = Guid.NewGuid(),
            role_name = request.RoleName
        };

        await roleRepo.AddAsync(role);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}