using FluentValidation;
using MediatR;
using PaperRockScissors_API.Models;
using PaperRockScissors_API.Services;
using System.Text.Json.Serialization;

namespace PaperRockScissors_API.Handlers
{
    public class PlayAgainstComputer
    {
        public class Command : IRequest<Response>
        {
            public Choice Player { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Player).NotEmpty();
                RuleFor(x => x.Player).Must(x => Enum.IsDefined(typeof(Choice), x)).WithMessage("Property must be in [1-5] range");
            }
        }

        public class HandlerAsync : IRequestHandler<Command, Response>
        {

            public ChoiceService _choiceService { get; set; }
            public HandlerAsync(ChoiceService choiceService)
            {
                _choiceService = choiceService;
            }


            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var computer = await _choiceService.GetRandomChoice();

                var game = new Game(2);
                game.AddPlayerChoice(new PlayerChoice("player", request.Player));
                game.AddPlayerChoice(new PlayerChoice("computer", computer));

                return new Response()
                {
                    Player = request.Player,
                    Computer = computer,
                    Results = game.Results.First().Result
                };
            }
        }

        public class Response
        {
            [JsonPropertyName("player")]
            public Choice Player { get; set; }
            [JsonPropertyName("computer")]
            public Choice Computer { get; set; }
            [JsonPropertyName("results")]
            public string Results { get; set; }
        }
    }
}
