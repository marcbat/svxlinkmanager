using DeviceId;

using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

using System.Reflection;

namespace SvxlinkManager.Telemetry
{
  public class SvxlinkManagerTelemetry : TelemetryInitializerBase
  {
    private readonly string deviceId;

    public SvxlinkManagerTelemetry(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
      deviceId = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString();
    }

    protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
    {
      telemetry.Context.GlobalProperties["DeviceId"] = deviceId;
      telemetry.Context.Device.Id = deviceId;
      telemetry.Context.Device.OperatingSystem = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
      telemetry.Context.Component.Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
      telemetry.Context.User.AuthenticatedUserId = platformContext.User?.Identity.Name ?? string.Empty;
    }
  }
}