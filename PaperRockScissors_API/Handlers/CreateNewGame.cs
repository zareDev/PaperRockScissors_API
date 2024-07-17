using FluentValidation;
using MediatR;
using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Handlers
{
    public class CreateNewGame
    {
        public class Command : IRequest<Response>
        {
            public int NumberOfPlayers { get; set; }
        }

        public class QueryValidator : AbstractValidator<Command>
        {
            public QueryValidator()
            {
                RuleFor(x => x.NumberOfPlayers).Must(x => x >= 2 && x <= 5).WithMessage("Property must be in [2-5] range");
            }
        }

        public class HandlerAsync : IRequestHandler<Command, Response>
        {
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {

                var result = PlayService.CreateMultiplayerGame(request.NumberOfPlayers);
                return new Response(result.Id);
            }
        }

        public record Response(Guid GameId);

    }
}
