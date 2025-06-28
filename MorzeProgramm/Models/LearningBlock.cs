using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MorzeProgramm.Models
{
    public class LearningBlock
    {
        [Key]
        public int BlockId { get; set; }

        public ICollection<Letter> Letters { get; set; } = new List<Letter>();

        public ICollection<UserProgress> UserProgresses { get; set; } = new List<UserProgress>();
    }
}