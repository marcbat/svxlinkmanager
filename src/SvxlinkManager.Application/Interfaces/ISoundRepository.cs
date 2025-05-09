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
    public interface ISoundRepository
    {
        Validation<Error, Unit> CreateAsyc(Sound sound);

        Validation<Error, Unit> UpdateAsyc(string name, Sound sound);

        Validation<Error, Unit> DeleteAsync(string name);
    }
}
