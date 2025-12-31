using Scores.Models;

namespace Scores.Interfaces
{
    public interface IScoreSortingStrategy
    {
        List<StudentScore> SortScores(List<StudentScore> scores);
    }
}