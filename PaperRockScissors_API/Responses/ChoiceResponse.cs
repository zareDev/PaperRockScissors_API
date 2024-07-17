using PaperRockScissors_API.Models;

namespace PaperRockScissors_API.Responses
{
    public class ChoiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ChoiceResponse(Choice choice)
        {
            Id = (int)choice;
            Name = choice.ToString().ToLower();
        }
    }
}
