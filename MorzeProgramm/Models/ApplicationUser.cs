using Microsoft.AspNetCore.Identity;

namespace MorzeProgramm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nickname { get; set; }

        // можно добавить позже: список прогрессов
        public ICollection<UserProgress> UserProgresses { get; set; }
    }
}
