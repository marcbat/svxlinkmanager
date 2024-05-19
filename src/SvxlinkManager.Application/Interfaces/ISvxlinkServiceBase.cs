using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISvxlinkServiceBase
    {

        IReadOnlyList<Node> Nodes { get; } 

        string Status { get; }

        ChannelBase? ActiveChannel { get; }

        void StartReflector(Reflector reflector, bool runAsDaemon = false, string logFile = null, string configFile = null, string pidFile = null, string runAs = null);

        void StartSvxlink(ChannelBase channel, bool runAsDaemon = false, string? logFile = null, string? configFile = null, string? pidFile = null, string? runAs = null);

        void StopReflector(Reflector reflector);

        void StopSvxlink();
    }
}