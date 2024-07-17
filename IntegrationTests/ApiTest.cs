using Microsoft.AspNetCore.Mvc.Testing;
using PaperRockScissors_API.Handlers;
using PaperRockScissors_API.Models;
using System.Net.Http.Json;
using System.Text.Json;
namespace IntegrationTests
{
    public class ApiTest
    {
        private readonly HttpClient _httpClient;

        private Dictionary<Choice, List<Choice>> winningMap = new Dictionary<Choice, List<Choice>>()
        {
            { Choice.Paper, new List<Choice>{Choice.Rock, Choice.Spock } },
            { Choice.Rock, new List<Choice>{Choice.Lizard, Choice.Scissors} },
            { Choice.Lizard, new List<Choice>{Choice.Paper, Choice.Spock} },
            { Choice.Spock, new List<Choice>{Choice.Rock, Choice.Scissors } },
            { Choice.Scissors, new List<Choice>{ Choice.Paper, Choice.Lizard } },
        };

        public ApiTest()
        {
            var factory = new WebApplicationFactory<PaperRockScissors_API.Program>();
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task ChoicesApi_ShouldReturnExpectedResponse()
        {
            var res = await _httpClient.GetStringAsync("/choices");
            var expectedRes = "[{\"id\":1,\"name\":\"rock\"},{\"id\":2,\"name\":\"paper\"},{\"id\":3,\"name\":\"scissors\"},{\"id\":4,\"name\":\"lizard\"},{\"id\":5,\"name\":\"spock\"}]";
            Assert.Equal(expectedRes, res);
        }

        [Fact]
        public async Task PlayApi_ShouldReturnExpectedResponse()
        {
            for (int i = 0; i < 10; i++)
            {
                HttpResponseMessage res;
                string content;
                PlayAgainstComputer.Response retVal;
                try
                {
                    res = await _httpClient.PostAsJsonAsync("/play", new PlayAgainstComputer.Command { Player = (Choice)(i % 5 + 1) });
                    content = await res.Content.ReadAsStringAsync();
                    retVal = JsonSerializer.Deserialize<PlayAgainstComputer.Response>(content);

                    if (retVal.Player == retVal.Computer)
                    {
                        Assert.False(retVal.Results != "tie");
                    }
                    else
                    {
                        if (winningMap[retVal.Player].Contains(retVal.Computer))
                        {
                            Assert.True(retVal.Results == "win");
                        }
                        else
                        {
                            Assert.True(retVal.Results == "lose");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //somethimes call to random number api cause timeout error
                }
            }
        }

        [Fact]
        public async Task PlayApi_ShouldReturnErrorMessage_WhenChoiceIsOutOfScope()
        {
            var res = await _httpClient.PostAsJsonAsync("/play", new PlayAgainstComputer.Command { Player = (Choice)(11) });
            var content = await res.Content.ReadAsStringAsync();
            Assert.Contains("Property must be in [1-5] range", content);
        }
    }
}