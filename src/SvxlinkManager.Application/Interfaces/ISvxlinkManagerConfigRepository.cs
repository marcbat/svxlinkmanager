using SvxlinkManager.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISvxlinkManagerConfigRepository
    {
        Task<SvxlinkManagerConfigAggregate> GetConfigAsync(Guid configId);

        Task UpdateAsync(SvxlinkManagerConfigAggregate config);
    }
}
