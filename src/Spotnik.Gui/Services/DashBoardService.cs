using Spotnik.Gui.Models;
using Spotnik.Gui.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Services
{
  public class DashBoardService
  {
    private readonly ChannelRepository channelRepository;

    public DashBoardService(ChannelRepository channelRepository)
    {
      this.channelRepository = channelRepository;
    }

    public IEnumerable<Channel> LoadChannels()
    {
      return channelRepository.GetChannels();
    }
  }
}
