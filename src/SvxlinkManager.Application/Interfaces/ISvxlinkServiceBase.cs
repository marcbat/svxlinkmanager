using SvxlinkManager.Domain.Entities;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISvxlinkServiceBase
    {
        List<Node> Nodes { get; set; }

        event Action<ChannelBase> Connected;
        event Action Disconnected;
        event Action<string, string> Error;
        event Action<Node> NodeConnected;
        event Action<Node> NodeDisconnected;
        event Action<Node> NodeRx;
        event Action<Node> NodeTx;

        Guid ActiveChannel { get; }

        void StartReflector(Reflector reflector, bool runAsDaemon = false, string logFile = null, string configFile = null, string pidFile = null, string runAs = null);

        void StartSvxlink(ChannelBase channel, bool runAsDaemon = false, string logFile = null, string configFile = null, string pidFile = null, string runAs = null);

        void StopReflector(Reflector reflector);

        void StopSvxlink();
    }
}