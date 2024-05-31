using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public abstract class Channel : ManagedChannel
    {
        public Channel(Guid id, string name, string soundGuid, string host, string callSign) : base(id, name, soundGuid)
        {
            
            Host = host;
            CallSign = callSign;
        }

        public string Host { get;}

        public string CallSign { get; set; }
    }
}
