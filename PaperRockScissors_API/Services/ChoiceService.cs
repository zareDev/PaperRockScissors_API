using PaperRockScissors_API.Models;
using System.Text.Json.Serialization;

namespace PaperRockScissors_API.Services
{
    public class ChoiceService
    {
        private readonly HttpClient _client;
        private readonly string _url = "https://codechallenge.boohma.com/random";
        public ChoiceService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Choice> GetRandomChoice()
        {
            var result = await _client.GetFromJsonAsync<RandomServiceResponse>(_url);
            if (result == null)
            {
                throw new Exception("Random service error");
            }
            return (Choice)(result.RandomNumber % 5 + 1);
        }

        private class RandomServiceResponse
        {
            [JsonPropertyName("random_number")]
            public int RandomNumber { get; set; }
        }
    }


}
