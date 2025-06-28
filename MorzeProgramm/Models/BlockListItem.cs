namespace MorzeProgramm.Models
{
    public class BlockListItem
    {
        public int BlockId { get; set; }
        public List<string> Letters { get; set; } = new();

        public float? ProgressPercent { get; set; }  // ← Добавлено
        public bool IsCompleted { get; set; } = false;
    }
}
