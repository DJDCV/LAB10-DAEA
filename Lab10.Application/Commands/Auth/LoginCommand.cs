using MediatR;
using Lab10.Application.DTOs;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;
using Lab10.Application.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab10.Application.Commands.Auth;
public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.Repository<user>();
            var userRoleRepo = _unitOfWork.Repository<user_role>();

            var users = await userRepo.GetAllAsync();
            var user = users.FirstOrDefault(u => u.email != null && u.email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.password_hash))
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