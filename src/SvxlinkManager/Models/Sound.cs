using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class Sound : IModelEntity
  {
    public int Id { get; set; }

    public int ChannelId { get; set; }

    public Channel Channel { get; set; }

    public string SoundName { get; set; }

    public byte[] SoundFile { get; set; }
  }
}