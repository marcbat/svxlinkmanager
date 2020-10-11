using Spotnik.Gui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.Services
{
  public interface IConfigService
  {
    Channel Get(string guid);

    List<Channel> GetChannel();
  }

  public class ConfigService : IConfigService
  {
    public Channel Get(string guid) => GetChannel().Single(c => c.Id == guid);

    public List<Channel> GetChannel()
    {
      return new List<Channel>
      {
        new Channel{ Id="f583e603-c628-4c00-8e47-6da6b1a4c7a5", Name ="Reseau des répéteurs francophones", SvxLinkFile = "svxlink.rrf", RestartFile = "restart.rrf"},
        new Channel{ Id = "0616a95c-2f62-433f-8b7a-06dd57d02b6f", Name ="Salon suisse romand", SvxLinkFile = "svxlink.reg", RestartFile = "restart.reg"},
      };
    }
  }
}
