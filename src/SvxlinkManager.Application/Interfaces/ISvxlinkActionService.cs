using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Interfaces
{
    public interface ISvxlinkActionService
    {
        event Action<ChannelBase> Connected;
        event Action<ChannelBase> Disconnected;
        event Action<string, string> Error;
        event Action<Node, bool> NodeConnected;
        event Action<Node> NodeDisconnected;
        event Action<Node> NodeRx;
        event Action<Node> NodeTx;
    }
}
