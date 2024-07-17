using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaperRockScissors_API.Handlers;
using PaperRockScissors_API.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PaperRockScissors_API.Controllers
{
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("choices")]
        public async Task<List<ChoiceResponse>> GetAllChoices()
        {
            return await _mediator.Send(new GetAllChoices.Query());
        }

        [HttpGet("choice")]
        public async Task<ChoiceResponse> Get()
        {
            return await _mediator.Send(new GetRandomChoice.Query());
        }
    }
}
