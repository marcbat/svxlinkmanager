using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Reflectors.Commands
{
    /// <summary>
    /// Command to update a reflector.
    /// </summary>
    public record UpdateReflectorCommand(Guid ConfigId, Guid ReflectorId, string Name, string Config) : IRequest<Unit>;

    /// <summary>
    /// Handler for the <see cref="UpdateReflectorCommand"/>.
    /// </summary>
    internal class UpdateReflectorCommandHandler : IRequestHandler<UpdateReflectorCommand, Unit>
    {
        private readonly ISvxlinkManagerConfigRepository _svxlinkManagerConfigRepository;
        private readonly ISvxlinkServiceBase svxlinkServiceBase;
        private readonly ILogger<UpdateReflectorCommandHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateReflectorCommandHandler"/> class.
        /// </summary>
        /// <param name="svxlinkManagerConfigRepository">The repository for managing SVXLink Manager configurations.</param>
        /// <param name="logger">The logger.</param>
        public UpdateReflectorCommandHandler(ISvxlinkManagerConfigRepository svxlinkManagerConfigRepository, ISvxlinkServiceBase svxlinkServiceBase,
                                             ILogger<UpdateReflectorCommandHandler> logger)
        {
            _svxlinkManagerConfigRepository = svxlinkManagerConfigRepository;
            this.svxlinkServiceBase = svxlinkServiceBase;
            this.logger = logger;
        }

        /// <summary>
        /// Handles the <see cref="UpdateReflectorCommand"/>.
        /// </summary>
        /// <param name="request">The update reflector command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<Unit> Handle(UpdateReflectorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Mise à jour d'un réflecteur.");

                var result = from config in _svxlinkManagerConfigRepository.GetConfig(request.ConfigId)
                             from _ in config.DeleteReflector(request.ReflectorId)
                             from reflector in Reflector.Create(request.ReflectorId, request.Name, request.Config)
                             from __ in config.AddReflector(reflector)
                             from ___ in _svxlinkManagerConfigRepository.UpdateAsync(config)
                             select config.Id;

                logger.LogInformation("Un réflecteur a été mis à jour avec succès.");

                if(reflector.Enable)
                    svxlinkServiceBase.StartReflector(reflector);

                logger.LogInformation("Le réflecteur a été démarré avec succès.");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Impossible de mettre à jour un réflecteur.");
                throw new SvxlinkManagerException("Impossible de mettre à jour un réflecteur.", ex);
            }
        }
    }
}
