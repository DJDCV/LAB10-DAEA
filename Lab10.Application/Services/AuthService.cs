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
        
        var user = await userRepo.GetByUsernameAsync(loginDto.Username);  

        if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.password_hash))
            return null!;

        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.username,
            Email = user.email
        };
    }

    public async Task<bool> RegisterAsync(UserDto userDto)
    {
        var userRepo = _unitOfWork.Repository<user>();
        
        var existingUser = (await userRepo.GetAllAsync()).FirstOrDefault(u => u.username == userDto.Username);
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

    private string GenerateJwtToken(user user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", user.user_id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }