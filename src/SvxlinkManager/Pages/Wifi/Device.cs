using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  public class Device
  {
    public string InUse { get; set; }

    public string Ssid { get; set; }

    public string Mode { get; set; }

    public string Channel { get; set; }

    public string Rate { get; set; }

    public string Signal { get; set; }

    public string Bars { get; set; }

    public string Security { get; set; }

    public string Password { get; set; }

    public bool HasConnection => Connection != null;

    public Connection Connection { get; set; }
  }
}