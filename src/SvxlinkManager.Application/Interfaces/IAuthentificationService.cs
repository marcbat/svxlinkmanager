using LanguageExt;
using LanguageExt.Common;

namespace SvxlinkManager.Application.Interfaces
{
    public interface IAuthentificationService
    {
        Validation<Error, string> SeedUser(string userName, string password);
    }
}