using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Scores.Models
{
    public class StudentSubject
    {
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }

        [JsonPropertyName("name")]
        public string StudentName { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("scores")]
        public List<StudentScore> Scores { get; set; }
    }
}