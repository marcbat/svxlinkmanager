﻿using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
  public class UserPasswordValidator : AbstractValidator<InstallerModel>
  {
    public UserPasswordValidator()
    {
      RuleFor(i => i.UserName).NotEmpty().WithMessage("Le nom d'utilisateur est obligatoire").EmailAddress().WithMessage("Le nom d'utilisateur doit être un email valide.");
      RuleFor(i => i.Password).NotEmpty().WithMessage("Le mot de passe est obligatoire").Length(6,20).WithMessage("Le mot de passe doit faire en 6 et 20 caractères");
      RuleFor(i => i.Password2).Must((i, p2) => i.Password == p2).WithMessage("Les deux mot de passe ne correspondent pas.");
    }
  }
}