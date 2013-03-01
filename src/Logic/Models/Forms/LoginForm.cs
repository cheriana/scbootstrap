namespace ScBootstrap.Logic.Models.Forms
{
    using System.ComponentModel.DataAnnotations;

    public class LoginForm
    {
        [Required(ErrorMessage = "Required"), Display(Name = "Your name"), StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required"), Display(Name = "Your password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool Persistent { get; set; }
    }
}
