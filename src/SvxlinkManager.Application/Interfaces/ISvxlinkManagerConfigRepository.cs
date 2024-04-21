using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISvxlinkManagerConfigRepository
    {
        Task Create(SvxlinkManagerConfigAggregate config);

        Task<SvxlinkManagerConfigAggregate> GetConfigAsync(Guid configId);

        Task UpdateAsync(SvxlinkManagerConfigAggregate config);

        Task<IEnumerable<SvxlinkChannel>> GetAllOriginalChannels();

        string GetDefaultSvxlinkConfig();
    }
}
