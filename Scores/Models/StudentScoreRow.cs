using System;
using System.Collections.Generic;
using System.Text;

namespace Scores.Models
{
    public class StudentScoreRow
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string LearningObjective { get; set; }
        public string Score { get; set; }
        public string Subject { get; set; }
    }
}
