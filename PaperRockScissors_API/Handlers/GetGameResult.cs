using FluentValidation;
using MediatR;
using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Handlers
{
    public class GetGameResult
    {
        public class Query : IRequest<Response>
        {
            public Guid GameId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.GameId).NotEmpty();
            }
        }

        public class HandlerAsync : IRequestHandler<Query, Response>
        {
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = PlayService.GetGameResult(request.GameId);
                if (result == null)
                {
                    return new Response("Game doesn't exist");
                }

                if (result.NumberOfPlayers != result.PlayersChoices.Count)
                {
                    return new Response("Waiting for all players to chose");
                }

                return new Response
                (
                    "Success",
                    result.Id,
                    result.Created,
                    result.GetStringResults()
                );
            }
        }
        public record Response(string Message, Guid? Id = null, DateTime? Created = null, List<string> Results = null);
    }
}
