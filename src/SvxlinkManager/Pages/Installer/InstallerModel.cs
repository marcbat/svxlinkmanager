﻿using SvxlinkManager.Models;
using SvxlinkManager.Pages.Updater;
using SvxlinkManager.Pages.Wifi;

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

    public Models.RadioProfile RadioProfile { get; set; } = new Models.RadioProfile { Name = "Profil principal", SquelchDetection = "GPIO", HasSa818 = true };

    public List<SvxlinkChannel> ChannelsToPreserved => Channels.Where(c => !ChannelsToDelete.Any(e => c.Equals(e))).ToList();

    public string ChannelsToPreservedList => String.Join(", ", ChannelsToPreserved.Select(x => x.Name));

    public bool UpdateToLastRelease { get; set; } = false;

    public Release LastRelease { get; set; }

    public string CurrentVersion { get; set; }

    public List<Device> Devices { get; set; }

    public Dictionary<string, string> TrackProperties
    {
      get
      {
        var installerModel = new Dictionary<string, string> {
        { nameof(UserName), UserName },
        { nameof(CallSign), CallSign },
        { nameof(AnnonceCallSign), AnnonceCallSign },
        { nameof(ChannelsToPreservedList), ChannelsToPreservedList },
        { nameof(UpdateToLastRelease), UpdateToLastRelease.ToString() },
        { nameof(LastRelease), LastRelease.Name },
        { nameof(CurrentVersion), CurrentVersion },
        };

        RadioProfile.TrackProperties.ToList().ForEach(x => installerModel.Add($"Radio{x.Key}", x.Value));

        DefaultChannel.TrackProperties.ToList().ForEach(x => installerModel.Add($"DefaultChannel{x.Key}", x.Value));

        return installerModel;
      }
    }
  }
}