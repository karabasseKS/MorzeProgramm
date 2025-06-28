namespace MorzeProgramm.Models
{
    public class BlockProgressModel
    {
        public int BlockId { get; set; }
        public List<string> Letters { get; set; } = new();

        public float ProgressPercent { get; set; }
        public bool IsCompleted { get; set; }

        public string DisplayProgress =>
            IsCompleted ? "✅ Пройден" :
            ProgressPercent > 0 ? $"{ProgressPercent:F0}%" :
            "—";

        public string ProgressCssClass =>
            IsCompleted ? "text-success" :
            ProgressPercent >= 50 ? "text-warning" :
            ProgressPercent > 0 ? "text-danger" :
            "text-muted";
    }
}
