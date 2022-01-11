using App.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Identity
{
    public class UsersClaimPrincipalFactory : UserClaimsPrincipalFactory<Users>
    {
        public UsersClaimPrincipalFactory(UserManager<Users> userManager, IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Users user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Member",user.Member));
            identity.AddClaim(new Claim("Organization", user.OrgId));
            return identity;
        }
    }
}
