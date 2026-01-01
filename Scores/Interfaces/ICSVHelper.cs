using Scores.Models;
using System.Runtime.CompilerServices;

namespace Scores.Interfaces
{
    public interface ICSVHelper
    {
        IAsyncEnumerable<StudentScoreRow> ReadCSVInChunksAsync(
            string filePath, 
            int chunkSize = 100);
        
        Task<List<StudentScoreRow>> ReadCSVAsync(string filePath);
    }
}
