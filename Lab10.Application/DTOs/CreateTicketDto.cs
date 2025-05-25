namespace Lab10.Application.DTOs;

public class CreateTicketDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = "abierto";
}