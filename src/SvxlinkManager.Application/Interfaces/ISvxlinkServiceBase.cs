using LanguageExt;
using LanguageExt.Common;

using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISvxlinkServiceBase
    {

        IReadOnlyList<Node> Nodes { get; } 

        string Status { get; }

        ChannelBase? ActiveChannel { get; }

        Validation<Error, Unit> StartReflector(Reflector reflector, bool runAsDaemon = false, string logFile = null, string configFile = null, string pidFile = null, string runAs = null);

        Validation<Error, Unit> StartSvxlink(ChannelBase channel, bool runAsDaemon = false, string? logFile = null, string? configFile = null, string? pidFile = null, string? runAs = null);

        Validation<Error, Unit> StopReflector(Reflector reflector);

        Validation<Error, Unit> StopSvxlink();
    }
}