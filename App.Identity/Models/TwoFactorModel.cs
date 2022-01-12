using System.ComponentModel.DataAnnotations;

namespace App.Identity.Models
{
    public class TwoFactorModel
    {
        [Required]
        public string Token { get; set; }
    }
}
