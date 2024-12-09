using AspNetCore.Identity.LiteDB.Models;

using LanguageExt;
using LanguageExt.Common;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Infrastructure.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AuthentificationService> logger;

        public AuthentificationService(UserManager<ApplicationUser> userManager, ILogger<AuthentificationService> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public Validation<Error, string> SeedUser(string userName, string password)
        {
            try
            {
                logger.LogInformation("Installation de l'utilisateur par défaut.");

                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName
                };

                var result = userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "Admin").Wait();

                //OnSetUser?.Invoke();

                return user.Id;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Erreur lors de l'intallation. {e.Message}");
                return Error.New("Impossible de créer l'utilisateur par défaut.", e);

            }
        }
    }
}
