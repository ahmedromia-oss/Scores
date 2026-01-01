using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scores.Models
{
    public sealed class StudentScoreRowMap : ClassMap<StudentScoreRow>
    {
        public StudentScoreRowMap()
        {
            Map(m => m.StudentId).Name("Student ID");
            Map(m => m.Name).Name("Name");
            Map(m => m.LearningObjective).Name("Learning Objective");
            Map(m => m.Score).Name("Score");
            Map(m => m.Subject).Name("Subject");
        }
    }
}
