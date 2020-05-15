using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using rn.gas.server.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace rn.gas.server.Services
{
   public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
   {
      public AppClaimsPrincipalFactory(
         UserManager<AppUser> userManager
         , RoleManager<IdentityRole> roleManager
         , IOptions<IdentityOptions> optionsAccessor)
         :base(userManager, roleManager, optionsAccessor)
      { }

      public async override Task<ClaimsPrincipal> CreateAsync(AppUser appUser) 
      {
         if (appUser == null) throw new ArgumentNullException( $" { nameof(appUser) } cannot be null." );
         if (string.IsNullOrWhiteSpace(appUser.UserName)) throw new ArgumentNullException($" { nameof(appUser.UserName) } cannot be null.");
         var principal = await base.CreateAsync(appUser);
         if (!string.IsNullOrWhiteSpace(appUser.FirstName))
         {
            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.GivenName, appUser.FirstName));
         }
         if (!string.IsNullOrWhiteSpace(appUser.LastName))
         {
            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Surname, appUser.LastName));
         }
         return principal;
      }

   }
}
