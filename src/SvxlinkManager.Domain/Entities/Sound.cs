using SvxlinkManager.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class Sound : AggregateRoot
    {
        public Sound(Guid id, string name, byte[] soundFile) : base(id)
        {
            Name = name;
            SoundFile = soundFile;
        }

        public string Name { get; }
        public byte[] SoundFile { get; }
    }
}
