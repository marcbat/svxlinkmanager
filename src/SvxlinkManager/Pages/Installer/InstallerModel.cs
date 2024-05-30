using SvxlinkManager.Domain.Entities;
using SvxlinkManager.Models;
using SvxlinkManager.Pages.Updater;

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

    public Models.SvxlinkChannel DefaultChannel { get; set; }

    public List<Models.SvxlinkChannel> Channels { get; set; }

    public List<Models.SvxlinkChannel> ChannelsToDelete { get; } = new List<Models.SvxlinkChannel>();

    public Models.RadioProfile RadioProfile { get; set; } = new Models.RadioProfile { Name = "Profil principal", SquelchDetection = "GPIO", HasSa818 = true };

    public List<Models.SvxlinkChannel> ChannelsToPreserved => Channels.Where(c => !ChannelsToDelete.Any(e => c.Equals(e))).ToList();

    public string ChannelsToPreservedList => String.Join(", ", ChannelsToPreserved.Select(x => x.Name));

    public bool UpdateToLastRelease { get; set; } = false;

    public Release LastRelease { get; set; }

    public string CurrentVersion { get; set; }

    public List<Device> Devices { get; set; }

  }
}