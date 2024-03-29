using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfig
{
    public record CreateSvxlinkManagerConfigCommand() : IRequest<Guid>;

    public class CreateSvxlinkManagerConfigCommandHandler : IRequestHandler<CreateSvxlinkManagerConfigCommand, Guid>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<CreateSvxlinkManagerConfigCommandHandler> logger;

        public CreateSvxlinkManagerConfigCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<CreateSvxlinkManagerConfigCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public async Task<Guid> Handle(CreateSvxlinkManagerConfigCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var config = SvxlinkManagerConfigAggregate.Create(Guid.NewGuid());
                await svxlinkManagerConfigRepository.Create(config);

                return config.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible de créer un nouveau svxlink manager config.");
                throw new SvxlinkManagerException("Impossible de créer un nouveau svxlink manager config.", ex);
            }
        }
    }
}
