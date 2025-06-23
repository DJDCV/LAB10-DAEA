using MediatR;

namespace Lab10.Application.DTOs;

public class CreateResponseDto: IRequest<bool>
{
    public Guid TicketId { get; set; }
    public Guid ResponderId { get; set; }
    public string Message { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}