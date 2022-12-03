using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class EcholinkChannel : Channel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string SysopName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int MaxQso { get; set; } = 1;

        [Required]
        public string Description { get; set; }

        [NotMapped]
        public override Dictionary<string, string> TrackProperties => new Dictionary<string, string>
        {
          {nameof(Name), Name },
          {nameof(CallSign), CallSign },
          {nameof(IsDefault), IsDefault.ToString() },
          {nameof(IsTemporized), IsTemporized.ToString()},
          {nameof(TimerDelay), TimerDelay.ToString()},
          {nameof(Dtmf), Dtmf.ToString() },
          {nameof(SysopName),SysopName },
          {nameof(MaxQso),MaxQso.ToString() },
        };
    }
}