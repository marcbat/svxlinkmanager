using Microsoft.AspNetCore.Identity;

namespace SvxlinkManager.Data
{
  public static class ApplicationDbInitializer
  {
    #region Methods

    /// <summary>
    /// If not exist, seed the admin user and add to Admin role.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    public static void SeedUsers(UserManager<IdentityUser> userManager)
    {
      if (userManager.FindByEmailAsync("admin@svxlinkmanager.com").Result == null)
      {
        IdentityUser user = new IdentityUser
        {
          UserName = "admin@svxlinkmanager.com",
          Email = "admin@svxlinkmanager.com"
        };

        IdentityResult result = userManager.CreateAsync(user, "Pa$$w0rd").Result;

        if (result.Succeeded)
        {
          userManager.AddToRoleAsync(user, "Admin").Wait();
        }
      }
    }

    #endregion Methods
  }
}