using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class ScanProfile : IModelEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ScanDelay { get; set; }

        public List<ManagedChannel> Channels { get; set; } = new List<ManagedChannel>();

        public bool Enable { get; set; }

        [NotMapped]
        public IDictionary<string, string> TrackProperties => new Dictionary<string, string>
    {
      { nameof(Name), Name},
      { nameof(ScanDelay), ScanDelay.ToString()},
      { nameof(Enable), Enable.ToString()},
      {nameof(Channels), string.Join(",",Channels.Select(c=>c.Name)) }
    };
    }
}