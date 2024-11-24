using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs
{
    public record CreateSvxlinkManagerConfigCommand(Guid ConfigId) : IRequest<Validation<Error, Guid>>;

    internal class CreateSvxlinkManagerConfigCommandHandler : IRequestHandler<CreateSvxlinkManagerConfigCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<CreateSvxlinkManagerConfigCommandHandler> logger;

        public CreateSvxlinkManagerConfigCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ILogger<CreateSvxlinkManagerConfigCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(CreateSvxlinkManagerConfigCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Création d'un nouveau svxlink manager config.");

            var result = from config in svxlinkManagerConfigRepository.FindConfig(request.ConfigId)
                         where config.IsNone
                         from newConfig in SvxlinkManagerConfigAggregate.Create(request.ConfigId)
                         from id in svxlinkManagerConfigRepository.Create(newConfig)
                         select id;

            logger.LogInformation("Création d'un nouveau svxlink manager config réussie.");

            return Task.FromResult(result);

        }
    }
}
