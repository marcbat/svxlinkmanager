using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Application.SvxlinkManagerConfigs.Channels.SvxlinkChannel.Queries;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System.Threading.Channels;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{
    public record InstallChannelsCommand(Guid ConfigId, IEnumerable<Guid> InstallChannels, string CallSign, string AnnonceCallSign) : IRequest<Validation<Error, Guid>>;

    internal class InstallChannelsCommandHandler : IRequestHandler<InstallChannelsCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly IMediator mediator;
        private readonly ILogger<InstallChannelsCommandHandler> logger;

        public InstallChannelsCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, IMediator mediator, ILogger<InstallChannelsCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<Validation<Error, Guid>> Handle(InstallChannelsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Début de l'installation des canaux.");

                var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from originalChannels in svxlinkManagerConfigRepository.GetAllOriginalChannels()
                             from existingChannels in FilterExisting(originalChannels, request.InstallChannels)
                             from channels in AddDefaultCall(existingChannels, request.CallSign, request.AnnonceCallSign)
                             from _ in UpdateChannelsInConfig(config, channels)
                             from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Les canaux ont été installés avec succès.");

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de l'installation des canaux.");

                throw new Exception("Erreur lors de l'installation des canaux.", ex);
            }


        }

        public static Validation<Error, List<SvxlinkChannel>> FilterExisting(List<SvxlinkChannel> svxlinkChannels, IEnumerable<Guid> guids)
        {
            var existingChannels = new List<SvxlinkChannel>();

            foreach (var guid in guids)
            {
                var channel = svxlinkChannels.FirstOrDefault(c => c.Id == guid);
                if (channel is not null)
                {
                    existingChannels.Add(channel);
                }
            }

            return existingChannels;
        }

        public static Validation<Error, List<SvxlinkChannel>> AddDefaultCall(List<SvxlinkChannel> svxlinkChannels, string callSign, string annonceCallSign)
        {
            foreach (var channel in svxlinkChannels)
            {
                channel.SetCallSign(callSign);
                channel.SetReportCallSign(annonceCallSign);
            }
            return svxlinkChannels;
        }

        public static Validation<Error, LanguageExt.Unit> UpdateChannelsInConfig(SvxlinkManagerConfigAggregate config, List<SvxlinkChannel> channels)
        {
            foreach (var channel in channels)
            {
                config.AddSvxlinkChannel(channel);
            }
            return LanguageExt.Unit.Default;
        }
    }

}
