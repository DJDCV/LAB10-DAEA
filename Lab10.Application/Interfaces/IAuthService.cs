using Lab10.Application.DTOs;

namespace Lab10.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
    Task<bool> RegisterAsync(UserDto userDto, string? roleName = null);
    Task<bool> ValidateUserAsync(string username, string password);
}