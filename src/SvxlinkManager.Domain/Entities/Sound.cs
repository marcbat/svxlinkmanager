using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities
{
    public class Sound
    {
        public Sound(int id, string name, byte[] soundFile)
        {
            Id = id;
            Name = name;
            SoundFile = soundFile;
        }

        public int Id { get; }
        public string Name { get; }
        public byte[] SoundFile { get; }
    }
}
