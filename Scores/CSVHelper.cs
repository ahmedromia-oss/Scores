using Scores.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scores
{
    public class CSVHelper
    {
        public List<StudentScoreRow> ReadCSV(string filePath, int chunkSize = 10)
        {
            var rows = new List<StudentScoreRow>();

            try
            {
                var lines = File.ReadLines(filePath).Skip(1); // Skip header
                var currentChunk = new List<string>();
                int lineNumber = 1;

                foreach (var line in lines)
                {
                    currentChunk.Add(line);
                    lineNumber++;
                    

                    if (currentChunk.Count >= chunkSize)
                    {
                        ProcessChunk(currentChunk, rows, lineNumber - chunkSize);
                        currentChunk.Clear();
                    }
                }

                // Process remaining lines
                if (currentChunk.Count > 0)
                {
                    ProcessChunk(currentChunk, rows, lineNumber - currentChunk.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                throw;
            }

            return rows;
        }

        private void ProcessChunk(List<string> chunk, List<StudentScoreRow> rows, int startLineNumber)
        {
            try
            {
                Console.WriteLine($"Processing chunk starting at line {startLineNumber}...");

                for (int i = 0; i < chunk.Count; i++)
                {
                    

                    var row = ParseLine(chunk[i]);
                    if (row != null)
                    {
                        rows.Add(row);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Skipped invalid line {startLineNumber + i}");
                    }
                }

                Console.WriteLine($"Successfully processed {chunk.Count} rows");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing chunk starting at line {startLineNumber}: {ex.Message}");
                Console.WriteLine("Stopping further processing due to error in chunk.");
                throw;
            }
        }

        private StudentScoreRow ParseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return null;

            var values = SplitCSVLine(line);

            if (values.Length < 5)
                return null;

            try
            {
                return new StudentScoreRow
                {
                    StudentId = int.Parse(values[0].Trim()),
                    Name = values[1].Trim(),
                    LearningObjective = values[2].Trim(),
                    Score = values[3].Trim(),
                    Subject = values[4].Trim()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: {line}. Error: {ex.Message}");
                return null;
            }
        }

        private string[] SplitCSVLine(string line)
        {
            var result = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }

            result.Add(currentField.ToString());
            return result.ToArray();
        }
    }
}