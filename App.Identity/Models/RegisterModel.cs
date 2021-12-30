using System.ComponentModel.DataAnnotations;

namespace App.Identity.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "{0} Campo obrigatório!")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O {0} deve conter entre {2} a {1} caracteres")]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} Campo obrigatório!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string  ConfgirmPassword { get; set; }
    }
}
