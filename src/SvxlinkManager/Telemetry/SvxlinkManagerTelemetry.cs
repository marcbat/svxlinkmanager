using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SvxlinkManager.Telemetry
{
  public class SvxlinkManagerTelemetry : ITelemetryInitializer
  {
    private string informationalVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public void Initialize(ITelemetry telemetry)
    {
      switch (telemetry)
      {
        case RequestTelemetry requestTelemetry:
          requestTelemetry.Properties["SvxlinkmanagerVersion"] = informationalVersion;
          break;

        case TraceTelemetry traceTelemetry:
          traceTelemetry.Properties["SvxlinkmanagerVersion"] = informationalVersion;
          break;

        default:
          break;
      }
    }
  }
}