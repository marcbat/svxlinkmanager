using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Data
{
  public static class ApplicationDbInitializer
  {
    public static void SeedUsers(UserManager<IdentityUser> userManager)
    {
      if (userManager.FindByEmailAsync("admin@svxlinkmanager.com").Result == null)
      {
        IdentityUser user = new IdentityUser
        {
          UserName = "admin@svxlinkmanager.com",
          Email = "admin@svxlinkmanager.com"
        };

        IdentityResult result = userManager.CreateAsync(user, "M162175m162175$").Result;

        if (result.Succeeded)
        {
          userManager.AddToRoleAsync(user, "Admin").Wait();
        }
      }
    }
  }
}
