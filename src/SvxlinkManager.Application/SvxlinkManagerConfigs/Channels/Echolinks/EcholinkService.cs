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

        internal Validation<Error, Guid> AddEcholink(Guid configId, Guid id, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
            from channel in EcholinkChannel.Create(Guid.NewGuid(), name, host, callSign, password, sysopName, location, maxQso, description)
            from _ in config.AddEcholinkChannel(channel)
            from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select config.Id;
        }

        internal Validation<Error, Guid> UpdateEcholink(Guid configId, Guid channelId, string name, string host, string callSign, string password, string sysopName, string location, int maxQso, string description)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
                   from channel in config.GetEcholinkChannel(channelId)
                   from _ in config.UpdateEcholinkChannel(channelId, name, host, callSign, password, sysopName, location, maxQso, description)
                   from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select config.Id;
        } 

        internal Validation<Error, Guid> DeleteEcholink(Guid configId, Guid channelId)
        {
            return from config in svxlinkManagerConfigRepository.GetConfig(configId)
                   from channel in config.GetEcholinkChannel(channelId)
                   from _ in config.DeleteEcholinkChannel(channelId)
                   from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                   select config.Id;
        }
    }
}
