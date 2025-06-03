using MediatR;
using Lab10.Application.DTOs;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Response;

public class CreateResponseCommand : IRequest<bool>
{
    public CreateResponseDto Response { get; set; } = null!;
}

public class CreateResponseCommandHandler : IRequestHandler<CreateResponseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateResponseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateResponseCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<response>();

        var newResponse = new response
        {
            response_id = Guid.NewGuid(),
            ticket_id = request.Response.TicketId,
            responder_id = request.Response.ResponderId,
            message = request.Response.Message,
            created_at = request.Response.CreatedAt ?? DateTime.UtcNow
        };

        await repo.AddAsync(newResponse);
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}