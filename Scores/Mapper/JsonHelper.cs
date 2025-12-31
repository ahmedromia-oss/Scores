using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scores.Models;

namespace Scores
{
    public class JsonHelper
    {
        // Convert StudentSubject list to JSON string
        public string ConvertToJson(List<StudentSubject> studentSubjects)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Pretty print
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // camelCase properties
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(studentSubjects, options);
        }

        // Save JSON to file
        public void SaveToFile(string json, string filePath)
        {
            System.IO.File.WriteAllText(filePath, json);
            Console.WriteLine($"JSON saved to: {filePath}");
        }

        // Display JSON in console
        public void DisplayJson(string json)
        {
            Console.WriteLine("Generated JSON:");
            Console.WriteLine(json);
        }
    }
}