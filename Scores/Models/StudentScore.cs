using System.Text.Json.Serialization;

namespace Scores.Models
{
    public class StudentScore
    {
        [JsonPropertyName("learning_objective")]
        public string LearningObjective { get; set; }

        [JsonPropertyName("score")]
        public string Score { get; set; }
    }
}