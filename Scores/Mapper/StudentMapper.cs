using System;
using System.Collections.Generic;
using System.Linq;
using Scores.Factories;
using Scores.Models;

namespace Scores
{
    public class StudentMapper
    {
        private readonly SortingStrategyFactory _strategyFactory;

        public StudentMapper()
        {
            _strategyFactory = new SortingStrategyFactory();
        }
        // Map list of StudentScoreRow to list of StudentSubject
        public List<StudentSubject> MapToStudentSubjects(List<StudentScoreRow> rows)
        {
            var studentSubjects = new List<StudentSubject>();

            // Group by StudentId and Subject (since one student can have multiple subjects)
            var grouped = rows.GroupBy(r => new { r.StudentId, r.Name, r.Subject });

            foreach (var group in grouped)
            {
                var studentSubject = new StudentSubject
                {
                    StudentId = group.Key.StudentId,
                    StudentName = group.Key.Name,
                    Subject = group.Key.Subject,
                    Scores = group.Select(r => new StudentScore
                    {
                        LearningObjective = r.LearningObjective,
                        Score = r.Score
                    }).ToList()
                };

                studentSubjects.Add(studentSubject);
            }

            return studentSubjects;
        }

        // Map and sort scores based on subject
        public List<StudentSubject> MapAndSortScores(List<StudentScoreRow> rows)
        {
            var studentSubjects = MapToStudentSubjects(rows);

            foreach (var student in studentSubjects)
            {
                var strategy = _strategyFactory.GetStrategy(student.Subject);
                student.Scores = strategy.SortScores(student.Scores);
            }

            return studentSubjects;
        }
    }
}