using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class AdvanceSvxlinkChannel : ManagedChannel
  {
    public string SvxlinkConf { get; set; }

    public string ModuleDtmfRepeater { get; set; }

    public string ModuleEchoLink { get; set; }

    public string ModuleFrn { get; set; }

    public string ModuleHelp { get; set; }

    public string ModuleMetarInfo { get; set; }

    public string ModuleParrot { get; set; }

    public string ModulePropagationMonitor { get; set; }

    public string ModuleSelCallEnc { get; set; }

    public string ModuleTclVoiceMail { get; set; }

    public string ModuleTrx { get; set; }

    public override Dictionary<string, string> TrackProperties => throw new NotImplementedException();
  }
}