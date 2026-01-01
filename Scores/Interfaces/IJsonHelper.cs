using Scores.Models;

namespace Scores.Interfaces
{
    public interface IJsonHelper
    {
        string ConvertToJson(List<StudentSubject> studentSubjects);
        Task SaveToFileAsync(string json, string filePath);
        void DisplayJson(string json);
    }
}

