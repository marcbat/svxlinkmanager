using LanguageExt;
using LanguageExt.Common;

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
        Validation<Error, Guid> Create(SvxlinkManagerConfigAggregate config);

        Validation<Error, SvxlinkManagerConfigAggregate> GetConfig(Guid configId);

        Validation<Error, Option<SvxlinkManagerConfigAggregate>> FindConfig(Guid configId);

        Validation<Error, Unit> UpdateAsync(SvxlinkManagerConfigAggregate config);

        Validation<Error, List<SvxlinkChannel>> GetAllOriginalChannels();

        string GetDefaultSvxlinkConfig();
    }
}
