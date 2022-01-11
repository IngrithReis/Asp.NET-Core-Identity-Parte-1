using System.ComponentModel.DataAnnotations;

namespace App.Identity.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
