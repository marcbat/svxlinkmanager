using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Aggregates
{
    public class AggregateRoot : Entity
    {
        public AggregateRoot()
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }
    }
}
