using LanguageExt;
using LanguageExt.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class EcholinkChannel : Channel
    {
        internal EcholinkChannel(Guid id, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description) : base(id, name, host, callSign)
        {
            Password = password;
            SysopName = sysopName;
            Location = location;
            MaxQso = maxQso;
            Description = description;
        }

        public static Validation<Error, EcholinkChannel> Create(Guid id, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description)
        {
            return (ValidateName(name), ValidateHost(host))
                .Apply((vname, vhost) => new EcholinkChannel(id, vname, vhost, callSign, password, sysopName, location, maxQso, description));
        }

        public string Password { get; }
        public string SysopName { get; }
        public string Location { get; }
        public int MaxQso { get; }
        public string Description { get; }
    }
}
