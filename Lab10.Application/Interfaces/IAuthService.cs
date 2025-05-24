using Lab10.Application.DTOs;

namespace Lab10.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
    Task<bool> RegisterAsync(UserDto userDto);
    Task<bool> ValidateUserAsync(string username, string password);
}