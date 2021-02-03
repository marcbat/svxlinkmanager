using DeviceId;

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

    private string deviceId = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString();

    public void Initialize(ITelemetry telemetry)
    {
      telemetry.Context.Device.Id = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString();
      telemetry.Context.Device.OperatingSystem = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
      telemetry.Context.Component.Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

      switch (telemetry)
      {
        case RequestTelemetry requestTelemetry:
          requestTelemetry.Properties["DeviceId"] = deviceId;
          break;

        case TraceTelemetry traceTelemetry:
          traceTelemetry.Properties["DeviceId"] = deviceId;
          break;

        case EventTelemetry eventTelementry:
          eventTelementry.Properties["DeviceId"] = deviceId;
          break;

        case PageViewTelemetry pageViewTelemetry:
          pageViewTelemetry.Properties["DeviceId"] = deviceId;
          break;

        default:
          break;
      }
    }
  }
}