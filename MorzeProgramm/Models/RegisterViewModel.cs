using System.ComponentModel.DataAnnotations;

namespace MorzeProgramm.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
