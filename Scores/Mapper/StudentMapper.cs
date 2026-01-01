using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Scores.Interfaces;
using Scores.Models;

namespace Scores
{
    public class StudentMapper : IStudentMapper
    {
        private readonly ISortingStrategyFactory _strategyFactory;
        private readonly ILogger<StudentMapper> _logger;

        public StudentMapper(ISortingStrategyFactory strategyFactory, ILogger<StudentMapper> logger)
        {
            _strategyFactory = strategyFactory ?? throw new ArgumentNullException(nameof(strategyFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<List<StudentSubject>> MapToStudentSubjectsAsync(List<StudentScoreRow> rows)
        {
            _logger.LogInformation("Mapping {Count} rows to student subjects", rows.Count);

            var studentSubjects = rows
                .GroupBy(r => new { r.StudentId, r.Name, r.Subject })
                .AsParallel()
                .Select(group => new StudentSubject
                {
                    StudentId = group.Key.StudentId,
                    StudentName = group.Key.Name,
                    Subject = group.Key.Subject,
                    Scores = group.Select(r => new StudentScore
                    {
                        LearningObjective = r.LearningObjective,
                        Score = r.Score
                    }).ToList()
                })
                .ToList();

            _logger.LogInformation("Mapped to {Count} student subjects", studentSubjects.Count);
            return Task.FromResult(studentSubjects);
        }

        public async Task<List<StudentSubject>> MapAndSortScoresAsync(List<StudentScoreRow> rows)
        {
            _logger.LogInformation("Starting mapping and sorting process for {Count} rows", rows.Count);

            var studentSubjects = await MapToStudentSubjectsAsync(rows);

            await Task.Run(() =>
            {
                System.Threading.Tasks.Parallel.ForEach(studentSubjects, student =>
                {
                    _logger.LogDebug("Sorting scores for student {StudentId} - {Subject}", student.StudentId, student.Subject);
                    var strategy = _strategyFactory.GetStrategy(student.Subject);
                    student.Scores = strategy.SortScores(student.Scores);
                });
            });

            _logger.LogInformation("Mapping and sorting completed successfully");
            return studentSubjects;
        }
    }
}
