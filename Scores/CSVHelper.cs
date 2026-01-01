using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Scores.Interfaces;
using Scores.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Scores
{
  

    public class CSVHelper : ICSVHelper
    {
        private readonly ILogger<CSVHelper> _logger;

        public CSVHelper(ILogger<CSVHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<StudentScoreRow>> ReadCSVAsync(string filePath)
        {
            ValidateFilePath(filePath);
            
            var rows = new List<StudentScoreRow>();
            await foreach (var row in ReadCSVInChunksAsync(filePath))
            {
                rows.Add(row);
            }
            return rows;
        }

        public async IAsyncEnumerable<StudentScoreRow> ReadCSVInChunksAsync(
            string filePath, 
            int chunkSize = 100)
        {
            ValidateFilePath(filePath);

            if (!File.Exists(filePath))
            {
                _logger.LogError("CSV file not found: {FilePath}", filePath);
                throw new FileNotFoundException($"CSV file not found: {filePath}");
            }

            _logger.LogInformation("Starting CSV processing from {FilePath} with chunk size {ChunkSize}", filePath, chunkSize);

            int processedCount = 0;
            int errorCount = 0;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = context =>
                {
                    errorCount++;
                    _logger.LogWarning("Bad data found at row {RowNumber}: {RawRecord}", 
                        context.Context.Parser.Row, context.RawRecord);
                }
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            
            csv.Context.RegisterClassMap<StudentScoreRowMap>();

            var records = csv.GetRecordsAsync<StudentScoreRow>();

            await foreach (var record in records)
            {

                if (record != null && IsValidRecord(record))
                {
                    processedCount++;
                    
                    if (processedCount % chunkSize == 0)
                    {
                        _logger.LogDebug("Processed {Count} records", processedCount);
                    }
                    
                    yield return record;
                }
                else
                {
                    errorCount++;
                    _logger.LogWarning("Skipped invalid record at row {RowNumber}", csv.Context.Parser.Row);
                }
            }

            _logger.LogInformation("CSV processing completed. Total rows processed: {Count}, Errors: {ErrorCount}", 
                processedCount, errorCount);
        }

        private void ValidateFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            var fullPath = Path.GetFullPath(filePath);
            if (fullPath != filePath && !Path.IsPathRooted(filePath))
            {
                _logger.LogWarning("Potential path traversal attempt detected: {FilePath}", filePath);
            }

            if (filePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("File path contains invalid characters.", nameof(filePath));
            }
        }

        private bool IsValidRecord(StudentScoreRow record)
        {
            return record.StudentId > 0 &&
                   !string.IsNullOrWhiteSpace(record.Name) &&
                   !string.IsNullOrWhiteSpace(record.Subject) &&
                   !string.IsNullOrWhiteSpace(record.Score) &&
                   !string.IsNullOrWhiteSpace(record.LearningObjective);
        }
    }
}