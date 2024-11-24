using LanguageExt;
using LanguageExt.Common;

using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Interfaces
{
    public interface IFileService
    {
        Validation<Error, Unit> WriteReflectorConfig(Reflector reflector);
    }
}
