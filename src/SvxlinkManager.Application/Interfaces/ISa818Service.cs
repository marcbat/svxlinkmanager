using LanguageExt;
using LanguageExt.Common;

using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISa818Service
    {
        Validation<Error, Unit> WriteRadioProfile(RadioProfil radioProfile);
    }
}