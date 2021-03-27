using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
  public class ChannelDefaultValidator : AbstractValidator<InstallerModel>
  {
    public ChannelDefaultValidator()
    {
      RuleFor(i => i.DefaultChannel).NotEmpty().WithMessage("Vous devez choisir un salon par défaut.");
    }
  }
}