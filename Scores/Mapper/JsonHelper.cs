using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Scores.Interfaces;
using Scores.Models;

namespace Scores
{
    public class JsonHelper : IJsonHelper
    {
        private readonly ILogger<JsonHelper> _logger;

        public JsonHelper(ILogger<JsonHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string ConvertToJson(List<StudentSubject> studentSubjects)
        {
            _logger.LogInformation("Converting {Count} student subjects to JSON", studentSubjects.Count);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(studentSubjects, options);
            _logger.LogDebug("JSON conversion completed successfully");
            return json;
        }

        public async Task SaveToFileAsync(string json, string filePath)
        {
            _logger.LogInformation("Saving JSON to file: {FilePath}", filePath);
            
            try
            {
                await File.WriteAllTextAsync(filePath, json);
                _logger.LogInformation("JSON successfully saved to: {FilePath}", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save JSON to file: {FilePath}", filePath);
                throw;
            }
        }

        public void DisplayJson(string json)
        {
            _logger.LogInformation("Displaying generated JSON:");
            Console.WriteLine(json);
        }
    }
}
