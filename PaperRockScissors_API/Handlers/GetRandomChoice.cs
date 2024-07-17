using MediatR;
using PaperRockScissors_API.Responses;
using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Handlers
{
    public class GetRandomChoice : IRequest<ChoiceResponse>
    {
        public class Query : IRequest<ChoiceResponse> { }

        public class HandlerAsync : IRequestHandler<Query, ChoiceResponse>
        {
            public ChoiceService _choiceService { get; set; }
            public HandlerAsync(ChoiceService choiceService)
            {
                _choiceService = choiceService;
            }
            public async Task<ChoiceResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _choiceService.GetRandomChoice();
                return new ChoiceResponse(result);
            }
        }
    }
}
