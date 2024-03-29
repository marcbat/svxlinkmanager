using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public abstract class ChannelBase : Entity
    {
        public ChannelBase(Guid id, string name) : base(id)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
