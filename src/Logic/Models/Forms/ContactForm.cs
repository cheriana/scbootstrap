namespace ScBootstrap.Logic.Models.Forms
{
    using System.ComponentModel.DataAnnotations;

    public class ContactForm
    {
        [Required(ErrorMessage = "Required"), Display(Name = "Your name"), StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required"), Display(Name = "Your email"), RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Required"), Display(Name = "Your comment"), StringLength(20, MinimumLength = 3, ErrorMessage = "Invalid enquiry")]
        public string Enquiry { get; set; }
    }
}
