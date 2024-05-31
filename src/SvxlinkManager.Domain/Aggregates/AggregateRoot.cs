using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Aggregates
{
    public abstract class AggregateRoot<TId> : Entity<TId>
    {
        public AggregateRoot()
        {
        }

        protected AggregateRoot(TId id) : base(id)
        {
        }
    }
}
