using Scores.Models;

namespace Scores.Interfaces
{
    public interface IStudentMapper
    {
        Task<List<StudentSubject>> MapToStudentSubjectsAsync(List<StudentScoreRow> rows);
        Task<List<StudentSubject>> MapAndSortScoresAsync(List<StudentScoreRow> rows);
    }
}

