using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Installer
{
  public class InstallerModel
  {
    [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    public string Password { get; set; }

    public string Password2 { get; set; }

    public string CallSign { get; set; }

    public string AnnonceCallSign { get; set; }

    public SvxlinkChannel DefaultChannel { get; set; }

    public List<SvxlinkChannel> Channels { get; set; }

    public List<SvxlinkChannel> ChannelsToDelete { get; } = new List<SvxlinkChannel>();

    public Models.RadioProfile RadioProfile { get; set; } = new Models.RadioProfile { SquelchDetection = "GPIO" };

    public List<SvxlinkChannel> ChannelsToPreserved => Channels.Where(c => !ChannelsToDelete.Any(e => c.Equals(e))).ToList();
  }
}