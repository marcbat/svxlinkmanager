using LanguageExt;
using LanguageExt.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class Reflector : Entity<Guid>
    {
        internal Reflector(Guid id, string name, string config) : base(id)
        {
            Name = name;
            Config = config;
        }

        public static Validation<Error, Reflector> Create(Guid id, string name, string config)
        {
            return new Reflector(id, name, config);
        }

        public string Name { get; }
        public string Config { get; }

        public bool Enable { get; set; }
        
    }
}
