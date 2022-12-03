using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
    public class RadioProfileValidator : AbstractValidator<Domain.Entities.RadioProfile>
    {
        public RadioProfileValidator()
        {
            RuleFor(r => r.RxFequ).NotEmpty().WithMessage("La fréquence Rx est obligatoire.")
              .Matches(new Regex("^[0-9]{3}.[0-9]{3}")).WithMessage("Le format de fréquence Rx n'est pas valide.");

            RuleFor(r => r.TxFrequ).NotEmpty().WithMessage("La fréquence Tx est obligatoire.")
              .Matches(new Regex("^[0-9]{3}.[0-9]{3}")).WithMessage("Le format de fréquence Tx n'est pas valide.");

            RuleFor(r => r.Squelch).NotEmpty();
            RuleFor(r => r.TxCtcss).Must((r, t) => r.Ctcss.Keys.Contains(t)).WithMessage("Le Tx Ctcss n'est pas valide.");
            RuleFor(r => r.RxCtcss).Must((r, t) => r.Ctcss.Keys.Contains(t)).WithMessage("Le Rx Ctcss n'est pas valide.");

            RuleFor(r => r.Volume).NotEmpty();
            RuleFor(r => r.PreEmph).NotEmpty();
            RuleFor(r => r.HightPass).NotEmpty();
            RuleFor(r => r.LowPass).NotEmpty();
            RuleFor(r => r.SquelchDetection).Must(s => s.Equals("CTCSS") || s.Equals("GPIO"));
        }
    }
}