using LanguageExt;
using LanguageExt.Common;

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
        private string name;
        private byte[] soundFile;

        internal Sound(string id, string name, byte[] soundFile) : base(id)
        {
            this.name = name;
            this.soundFile = soundFile;
        }

        public static Validation<Error, Sound> Create(string id, string name, byte[] soundFile)
        {
            return (ValidateName(name), ValidateSoundFile(soundFile))
                .Apply((n, s) => new Sound(id, n, s));

        }

        public string Name => name;

        public static Validation<Error, string> ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Error.New("Le nom du son est obligatoire.");
            
            return name;
        }

        public byte[] SoundFile => soundFile;

        public static Validation<Error, byte[]> ValidateSoundFile(byte[] soundFile)
        {
            if (soundFile.Length == 0)
                return Error.New("Un fichier son est obligatoire.");

            return soundFile;
        }
    }
}
