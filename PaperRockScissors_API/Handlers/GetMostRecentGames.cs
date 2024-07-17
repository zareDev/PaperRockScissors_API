using MediatR;
using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Handlers
{
    public class GetMostRecentGames
    {
        public class Query : IRequest<List<Response>>
        {
            public int NumberOfGames { get; set; }
        }

        public class HandlerAsync : IRequestHandler<Query, List<Response>>
        {
            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = new List<Response>(request.NumberOfGames);
                var node = PlayService.GameResults.First;
                for (int i = 0; i < request.NumberOfGames; i++)
                {
                    if (node == null) break;

                    var res = node.Value;
                    result.Add(new Response(res.Id, res.Created, res.GetStringResults()));
                    node = node.Next;
                }

                return result;
            }
        }
        public record Response(Guid Id, DateTime Created, List<string> Results);

    }
}
