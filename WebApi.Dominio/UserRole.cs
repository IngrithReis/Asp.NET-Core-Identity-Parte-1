using Microsoft.AspNetCore.Identity;

namespace WebApi.Dominio
{
    public class UserRole : IdentityUserRole<int>
    {   
        // classe de relacionamento entre usuário e role (n -n)
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
