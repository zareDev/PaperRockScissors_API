using MediatR;
using PaperRockScissors_API.Models;
using PaperRockScissors_API.Responses;

namespace PaperRockScissors_API.Handlers
{
    public class GetAllChoices
    {
        public class Query : IRequest<List<ChoiceResponse>> { }

        public class HandlerAsync : IRequestHandler<Query, List<ChoiceResponse>>
        {
            public async Task<List<ChoiceResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = Enum.GetValues(typeof(Choice)).Cast<Choice>();
                return result.Select(x => new ChoiceResponse(x)).ToList();
            }
        }
    }
}
