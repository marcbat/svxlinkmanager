using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
  public class ChannelChooseValidator : AbstractValidator<InstallerModel>
  {
    public ChannelChooseValidator()
    {
      RuleFor(i => i.CallSign).NotEmpty().WithMessage("L'indicatif est obligatoire.");
      RuleFor(i => i.AnnonceCallSign).NotEmpty().WithMessage("L'indicatif annoncé est obligatoire.");
      RuleFor(i => i.Channels).Must((i, c) => c.Count > i.ChannelsToDelete.Count).WithMessage("Vous devez concerver au moins un salon.");
    }
  }
}