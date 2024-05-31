using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class ScanProfile : Entity<Guid>
    {
        public ScanProfile(Guid id, string name, int scanDelay): base(id)
        {
            Name = name;
            ScanDelay = scanDelay;
        }

        public string Name { get; }

        public int ScanDelay { get; }

        public bool Enable { get; set; }
    }
}
