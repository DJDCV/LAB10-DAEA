using Lab10.Application.DTOs;
using Lab10.Application.Helpers;
using Lab10.Application.Interfaces;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab10.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        var userRepo = _unitOfWork.Repository<user>();
        var userRoleRepo = _unitOfWork.Repository<user_role>();
        
        var users = await userRepo.GetAllAsync();
        var user = users.FirstOrDefault(u => u.email != null && u.email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase));

        if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.password_hash))
            return null!;
        
        var userRoles = await userRoleRepo.GetAllAsync();
        var rolesForUser = userRoles
            .Where(ur => ur.user_id == user.user_id && ur.role != null)
            .Select(ur => ur.role.role_name)
            .ToList();

        var token = GenerateJwtToken(user, rolesForUser);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.username,
            Email = user.email
        };
    }

    public async Task<bool> RegisterAsync(UserDto userDto, string? roleName = null)
    {
        var userRepo = _unitOfWork.Repository<user>();
        var userRoleRepo = _unitOfWork.Repository<user_role>();
        var roleRepo = _unitOfWork.Repository<role>();

        var existingUser = (await userRepo.GetAllAsync())
            .FirstOrDefault(u => u.username == userDto.Username);
        if (existingUser != null) return false;

        var user = new user
        {
            user_id = Guid.NewGuid(),
            username = userDto.Username,
            password_hash = PasswordHasher.HashPassword(userDto.Password),
            email = userDto.Email,
            created_at = DateTime.UtcNow
        };

        await userRepo.AddAsync(user);

        if (!string.IsNullOrEmpty(roleName))
        {
            var role = (await roleRepo.GetAllAsync())
                .FirstOrDefault(r => r.role_name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
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
    
    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        var userRepo = _unitOfWork.Repository<user>();

        var user = (await userRepo.GetAllAsync()).FirstOrDefault(u => u.username == username);
        if (user == null)
            return false;

        return PasswordHasher.VerifyPassword(password, user.password_hash);
    }

    private string GenerateJwtToken(user user, IList<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", user.user_id.ToString())
        };

        // Agregar roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
