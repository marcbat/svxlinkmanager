using SvxlinkManager.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class Sound : AggregateRoot<string>
    {
        public Sound(string id, string name, byte[] soundFile) : base(id)
        {
            Name = name;
            SoundFile = soundFile;
        }

        public string Name { get; }
        public byte[] SoundFile { get; }
    }
}
