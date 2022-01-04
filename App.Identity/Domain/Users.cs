using Microsoft.AspNetCore.Identity;

namespace App.Identity.Domain
{
    public class Users : IdentityUser
    {
        public string NomeCompleto { get; set; }
        

    }
}
