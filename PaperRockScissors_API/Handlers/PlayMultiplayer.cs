using FluentValidation;
using MediatR;
using PaperRockScissors_API.Models;
using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Handlers
{
    public class PlayMultiplayer
    {
        public class Command : IRequest<Response>
        {
            public Guid GameId { get; set; }
            public string Player { get; set; }
            public Choice Choice { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.GameId).NotEmpty();
                RuleFor(x => x.Player).NotEmpty();
                RuleFor(x => x.Choice).Must(x => Enum.IsDefined(typeof(Choice), x)).WithMessage("Property must be in [1-5] range");
            }
        }

        public class HandlerAsync : IRequestHandler<Command, Response>
        {
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var success = PlayService.SetPlayerChoiceForTheGame(request.GameId, request.Player, request.Choice);

                if (success == null)
                {
                    return new Response()
                    {
                        Message = "Game doesn't exist"
                    };
                }

                if (success.Value)
                {
                    return new Response()
                    {
                        Message = "Success. Check results."
                    };
                }
                else
                {
                    return new Response()
                    {
                        Message = "Game closed. Check results."
                    };
                }
            }
        }

        public class Response
        {
            public string Message { get; set; }
        }

    }
}
