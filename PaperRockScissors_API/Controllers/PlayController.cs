using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaperRockScissors_API.Handlers;

namespace PaperRockScissors_API.Controllers
{
    [ApiController]
    public class PlayController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlayController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("play")]
        public async Task<PlayAgainstComputer.Response> PlayAgainstComputer(PlayAgainstComputer.Command req)
        {
            return await _mediator.Send(req);
        }

        [HttpGet("recentGames")]
        public async Task<List<GetMostRecentGames.Response>> GetMostRecentPlays(int numberOfGames = 10)
        {
            return await _mediator.Send(new GetMostRecentGames.Query { NumberOfGames = numberOfGames });
        }

        [HttpDelete("recentGames")]
        public async Task ResetGamesList()
        {
            await _mediator.Send(new DeleteRecentGamesList.Command());
        }

        [HttpPost("createMultiplayer")]
        public async Task<CreateNewGame.Response> CreateMultiplayerGame(CreateNewGame.Command req)
        {
            return await _mediator.Send(req);
        }

        [HttpPost("playMultiplayer")]
        public async Task<PlayMultiplayer.Response> PlayMultiplayerGame(PlayMultiplayer.Command req)
        {
            return await _mediator.Send(req);
        }

        [HttpGet("gameResult")]
        public async Task<GetGameResult.Response> GetResult(Guid gameId)
        {
            return await _mediator.Send(new GetGameResult.Query { GameId = gameId });
        }
    }
}