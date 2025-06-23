using MediatR;
using Lab10.Application.DTOs;
using Lab10.Domain.Entities;
using Lab10.Domain.Interfaces;

namespace Lab10.Application.Commands.Response
{
    public class CreateResponseDtoHandler : IRequestHandler<CreateResponseDto, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateResponseDtoHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateResponseDto request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<response>();

            var newResponse = new response
            {
                response_id = Guid.NewGuid(),
                ticket_id = request.TicketId,
                responder_id = request.ResponderId,
                message = request.Message,
                created_at = request.CreatedAt ?? DateTime.UtcNow
            };

            await repo.AddAsync(newResponse);
            var result = await _unitOfWork.CommitAsync();

            return result > 0;
        }
    }
}