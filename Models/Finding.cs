namespace SACSharp.Models
{
    public class Finding
    {
        public string RuleId { get; set; } = "";
        public string FilePath { get; set; } = "";
        public int LineNumber { get; set; }
        public string Message { get; set; } = "";
        public string Resolution { get; set; } = "";
        public Severity Severity { get; set; } = Severity.Low;
    }
}
