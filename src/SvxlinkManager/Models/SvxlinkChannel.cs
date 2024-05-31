using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class SvxlinkChannel : Channel
  {
    public string AuthKey { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string ReportCallSign { get; set; }

    public static implicit operator SvxlinkChannel(Domain.Entities.SvxlinkChannel svxlinkChannel)
    {
            var channel = new SvxlinkChannel
            {
                Id = svxlinkChannel.Id,
                Name = svxlinkChannel.Name,
                AuthKey = svxlinkChannel.AuthKey,
                Host = svxlinkChannel.Host,
                Port = svxlinkChannel.Port,
                CallSign = svxlinkChannel.CallSign,
                ReportCallSign = svxlinkChannel.ReportCallSign,
               
            };

            if (svxlinkChannel.SoundGuid is not null)
                channel.Sound = new Sound { SoundName = new FileInfo(svxlinkChannel.SoundGuid).Name };

            return channel;
    }

  }
}