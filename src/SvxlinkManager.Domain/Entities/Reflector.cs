using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class Reflector : Entity<Guid>
    {
        public Reflector(Guid id, string name, string config) : base(id)
        {
            Name = name;
            Config = config;
        }

        public string Name { get; }
        public string Config { get; }

        public bool Enable { get; set; }
        
    }
}
