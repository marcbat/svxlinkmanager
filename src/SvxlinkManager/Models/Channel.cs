using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SvxlinkManager.Models
{
    public abstract class Channel : ManagedChannel
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public string CallSign { get; set; }

        public List<ScanProfile> ScanProfiles { get; set; }

        
    }
}