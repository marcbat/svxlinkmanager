using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class SvxlinkChannel(Guid id, string name, Guid soundGuid, string host, int port, string callSign, string reportCallSign) : Channel(id, name, soundGuid, host, callSign)
    {
    public string? AuthKey { get; set; }

    public int Port { get; } = port;

    public string ReportCallSign { get; set; } = reportCallSign;
  }
}
