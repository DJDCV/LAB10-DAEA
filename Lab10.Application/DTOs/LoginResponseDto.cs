namespace Lab10.Application.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
}