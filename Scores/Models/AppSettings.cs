namespace Scores.Models
{
    public class AppSettings
    {
        public string DefaultInputPath { get; set; } = "scores.csv";
        public string DefaultOutputPath { get; set; } = "output.json";
        public int ChunkSize { get; set; } = 100;
        public bool EnableAutoSave { get; set; } = false;
    }
}

