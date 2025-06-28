namespace MorzeProgramm.Models
{
    public class Letter
    {
        public int Id { get; set; }

        public string Value { get; set; }

        // Навигация обратно к блоку
        public int LearningBlockId { get; set; }
        public LearningBlock LearningBlock { get; set; }
    }
}