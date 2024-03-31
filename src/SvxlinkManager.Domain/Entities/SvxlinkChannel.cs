using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class SvxlinkChannel : Channel
    {
        public SvxlinkChannel(Guid id, string name, Guid soundGuid, string host, string callSign, int port, string reportCallSign) : base(id, name, soundGuid, host, callSign)
        {
            Port = port;
            ReportCallSign = reportCallSign;
        }

        public string? AuthKey { get; set; }

        public int Port { get; }

        public string ReportCallSign { get; set; }
    }
}
