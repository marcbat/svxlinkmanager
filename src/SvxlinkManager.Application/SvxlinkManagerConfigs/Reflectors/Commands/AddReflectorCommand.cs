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
    public record AddReflectorCommand(Guid ConfigId, string Name, string Config) : IRequest<Guid>;

    /// <summary>
    /// Handler for the AddReflectorCommand.
    /// </summary>
    internal class AddReflectorCommandHandler : IRequestHandler<AddReflectorCommand, Guid>
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
        public async Task<Guid> Handle(AddReflectorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                SvxlinkManagerConfigAggregate config = await svxlinkManagerConfigRepository.GetConfigAsync(request.ConfigId);

                var reflectorGuid = Guid.NewGuid();
                var reflector = new Reflector(reflectorGuid, request.Name, request.Config);

                config.AddReflector(reflector);

                await svxlinkManagerConfigRepository.UpdateAsync(config);

                logger.LogInformation("Un nouveau réflecteur a été ajouté avec succès.");

                return reflectorGuid;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible d'ajouter un nouveau réflecteur.");
                throw new SvxlinkManagerException("Unable to add a new reflector.", ex);
            }
        }
    }
}
