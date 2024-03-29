using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Aggregates
{
    

    public class SvxlinkManagerConfigAggregate
    {
        private readonly List<SvxlinkChannel> _svxlinkChannels = [];

        public IReadOnlyCollection<SvxlinkChannel> SvxlinkChannels => _svxlinkChannels.AsReadOnly();

        public void AddSvxlinkChannel(SvxlinkChannel svxlinkChannel)
        {
            _svxlinkChannels.Add(svxlinkChannel);
        }
    }
}
