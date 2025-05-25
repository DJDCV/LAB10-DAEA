using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Services;

public class RoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> AssignRoleToUser(Guid userId, Guid roleId)
    {
        var userRoleRepo = _unitOfWork.Repository<user_role>();
        
        var existing = (await userRoleRepo.GetAllAsync())
            .FirstOrDefault(ur => ur.user_id == userId && ur.role_id == roleId);

        if (existing != null)
            return false;

        var userRole = new user_role
        {
            user_id = userId,
            role_id = roleId,
            assigned_at = DateTime.UtcNow
        };

        await userRoleRepo.AddAsync(userRole);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
    
    public async Task<List<string>> GetRolesForUser(Guid userId)
    {
        var userRoleRepo = _unitOfWork.Repository<user_role>();
        var userRoles = await userRoleRepo.GetAllAsync();

        return userRoles
            .Where(ur => ur.user_id == userId)
            .Select(ur => ur.role.role_name)
            .ToList();
    }
    
    public async Task<bool> CreateRoleAsync(string roleName)
    {
        var roleRepo = _unitOfWork.Repository<role>();
        
        var existingRole = (await roleRepo.GetAllAsync())
            .FirstOrDefault(r => r.role_name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

        if (existingRole != null)
            return false;

        var role = new role
        {
            role_id = Guid.NewGuid(),
            role_name = roleName
        };

        await roleRepo.AddAsync(role);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }

    public async Task<List<string>> GetAllRolesAsync()
    {
        var roleRepo = _unitOfWork.Repository<role>();
        var roles = await roleRepo.GetAllAsync();
        return roles.Select(r => r.role_name).ToList();
    }

}