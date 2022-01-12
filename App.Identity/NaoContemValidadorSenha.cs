using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace App.Identity
{
    public class NaoContemValidadorSenha<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var username = await manager.GetUserNameAsync(user);

            if (username == password)
            {
                return IdentityResult.Failed
                    (new IdentityError { Description = "A senha não pode ser igual ao nome de usuário" }
                    );

                if (password.Contains("password") && password.Contains("senha"))
                {

                    return IdentityResult.Failed
                     (new IdentityError { Description = "A senha não pode conter o nome password ou senha" }
                     );

                }

            }

            return IdentityResult.Success;
        }
    }
}
