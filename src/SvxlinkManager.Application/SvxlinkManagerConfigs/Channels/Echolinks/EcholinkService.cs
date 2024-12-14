using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;
using SvxlinkManager.Application.Interfaces;

using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.Echolinks
{
    internal class EcholinkService
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ISoundRepository soundRepository;

        public EcholinkService(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISoundRepository soundRepository)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.soundRepository = soundRepository;
        }

        internal Validation<Error, Guid> AddEcholink(Guid configId, string soundName, byte[] soundFile, Guid id, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
            from Sound in CreateSound(soundName, soundFile)
            from channel in EcholinkChannel.Create(Guid.NewGuid(), name, host, callSign, password, sysopName, location, maxQso, description)
            from _ in config.AddEcholinkChannel(channel)
            from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select config.Id;
        }

        private Validation<Error, Unit> CreateSound(string soundName, byte[] soundFile)
        {
            return Sound.Create($"$/sounds/{soundName}", soundName, soundFile)
                .Bind(soundRepository.CreateAsyc);
        }
    }
}
