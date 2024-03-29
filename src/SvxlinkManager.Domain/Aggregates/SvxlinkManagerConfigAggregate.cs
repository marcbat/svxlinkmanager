using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Aggregates
{
    

    public class SvxlinkManagerConfigAggregate : AggregateRoot
    {
        private List<SvxlinkChannel> _svxlinkChannels = [];

        protected SvxlinkManagerConfigAggregate(Guid id) : base(id)
        {
        }

        public SvxlinkManagerConfigAggregate()
        { 
        }

        public static SvxlinkManagerConfigAggregate Create(Guid id)
        {
            return new SvxlinkManagerConfigAggregate(id);
        }

        public IReadOnlyCollection<SvxlinkChannel> SvxlinkChannels{
            get=>  _svxlinkChannels.AsReadOnly(); 
            private set => _svxlinkChannels = value.ToList();
        } 

        public void AddSvxlinkChannel(SvxlinkChannel svxlinkChannel)
        {
            _svxlinkChannels.Add(svxlinkChannel);
        }

        public void DeleteSvxlinkChannel(Guid channelId)
        {
            var svxlinkChannel = _svxlinkChannels.FirstOrDefault(x => x.Id == channelId) ?? throw new Exception("Svxlink channel not found");

            _svxlinkChannels.Remove(svxlinkChannel);
        }
    }
}
