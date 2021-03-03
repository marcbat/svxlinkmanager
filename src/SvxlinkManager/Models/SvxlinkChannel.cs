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

    [NotMapped]
    public override Dictionary<string, string> TrackProperties => new Dictionary<string, string> {
      {nameof(Name), Name },
      {nameof(CallSign), CallSign },
      {nameof(IsDefault), IsDefault.ToString() },
      {nameof(IsTemporized), IsTemporized.ToString()},
      {nameof(TimerDelay), TimerDelay.ToString()},
      {nameof(Dtmf), Dtmf.ToString() },
      {nameof(ReportCallSign),ReportCallSign },
    };
  }
}