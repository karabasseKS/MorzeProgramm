namespace MorzeProgramm.Models
{
    public class UserProgress
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int BlockId { get; set; }
        public LearningBlock Block { get; set; }

        public float ProgressPercent { get; set; }
        public bool IsCompleted { get; set; }
    }
}
