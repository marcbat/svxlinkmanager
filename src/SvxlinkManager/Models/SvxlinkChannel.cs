using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
      return new SvxlinkChannel
      {
        Id = svxlinkChannel.Id,
        Name = svxlinkChannel.Name,
        AuthKey = svxlinkChannel.AuthKey,
        Port = svxlinkChannel.Port,
        ReportCallSign = svxlinkChannel.ReportCallSign
      };
    }

  }
}