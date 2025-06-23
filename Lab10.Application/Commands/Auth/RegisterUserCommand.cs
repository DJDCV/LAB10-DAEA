using MediatR;
using Lab10.Application.DTOs;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;
using Lab10.Application.Helpers;

namespace Lab10.Application.Commands.Auth;

public class RegisterUserCommand : IRequest<bool>
{
    public UserDto User { get; set; } = null!;
    public string? RoleName { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userRepo = _unitOfWork.Repository<user>();
        var userRoleRepo = _unitOfWork.Repository<user_role>();
        var roleRepo = _unitOfWork.Repository<role>();

        var existingUser = (await userRepo.GetAllAsync())
            .FirstOrDefault(u => u.username == request.User.Username);
        if (existingUser != null) return false;

        var user = new user
        {
            user_id = Guid.NewGuid(),
            username = request.User.Username,
            password_hash = PasswordHasher.HashPassword(request.User.Password),
            email = request.User.Email,
            created_at = DateTime.UtcNow
        };

        await userRepo.AddAsync(user);

        if (!string.IsNullOrEmpty(request.RoleName))
        {
            var role = (await roleRepo.GetAllAsync())
                .FirstOrDefault(r => r.role_name.Equals(request.RoleName, StringComparison.OrdinalIgnoreCase));
            if (role != null)
            {
                var userRole = new user_role
                {
                    user_id = user.user_id,
                    role_id = role.role_id,
                    assigned_at = DateTime.UtcNow
                };
                await userRoleRepo.AddAsync(userRole);
            }
        }

        var result = await _unitOfWork.CommitAsync();
        return result > 0;
    }
}