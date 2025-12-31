using System;
using Scores;
using Scores.Models;

namespace Scores
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1. Read CSV
                var csvHelper = new CSVHelper();
                var rows = csvHelper.ReadCSV(@"D:\scores.csv");

                // 2. Map to StudentSubject objects
                var mapper = new StudentMapper();
                var studentSubjects = mapper.MapAndSortScores(rows);

                // 3. Convert to JSON
                var jsonHelper = new JsonHelper();
                string json = jsonHelper.ConvertToJson(studentSubjects);

                // 4. Display JSON
                jsonHelper.DisplayJson(json);

                // 5. Save to file
                //jsonHelper.SaveToFile(json, @"D:\output.json");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}