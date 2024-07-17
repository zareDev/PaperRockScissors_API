using MediatR;
using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Handlers
{
    public class DeleteRecentGamesList
    {
        public class Command : IRequest<Unit>
        {
        }

        public class HandlerAsync : IRequestHandler<Command, Unit>
        {
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                PlayService.GameResults.Clear();
                return Unit.Value;
            }
        }

    }
}
