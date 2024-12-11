using LanguageExt;
using LanguageExt.Common;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands
{
    /// <summary>
    /// Command to add a new reflector.
    /// </summary>
    public record AddReflectorCommand(Guid ConfigId, string Name, string Config) : IRequest<Validation<Error, Guid>>;

    /// <summary>
    /// Handler for the AddReflectorCommand.
    /// </summary>
    internal class AddReflectorCommandHandler : IRequestHandler<AddReflectorCommand, Validation<Error, Guid>>
    {
        private readonly ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository;
        private readonly ILogger<AddReflectorCommandHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddReflectorCommandHandler"/> class.
        /// </summary>
        /// <param name="svxlinkManagerConfigRepository">The repository for SvxlinkManagerConfig.</param>
        /// <param name="logger">The logger.</param>
        public AddReflectorCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository,
                                          ILogger<AddReflectorCommandHandler> logger)
        {
            this.svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Handles the AddReflectorCommand.
        /// </summary>
        /// <param name="request">The AddReflectorCommand.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The Guid of the newly added reflector.</returns>
        public Task<Validation<Error, Guid>> Handle(AddReflectorCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Ajout d'un nouveau réflecteur.");

            var result = from config in svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                         from reflector in Reflector.Create(Guid.NewGuid(), request.Name, request.Config)
                         from _ in config.AddReflector(reflector)
                         from __ in svxlinkManagerConfigRepository.UpdateAsync(config)
                         select reflector.Id;

            logger.LogInformation("Un nouveau réflecteur a été ajouté avec succès.");

            return Task.FromResult(result);

        }
    }
}
