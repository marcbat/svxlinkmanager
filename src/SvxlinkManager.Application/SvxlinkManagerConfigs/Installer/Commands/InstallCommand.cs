using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Pipes;

using MediatR;

using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SvxlinkManager.Application.SvxlinkManagerConfigs.Installer.Commands
{

    public record InstallCommand(string UserName, 
                                 string Password,    
                                 Guid ConfigId,
                                 IEnumerable<Guid> InstallChannels,
                                 string CallSign,
                                 string AnnonceCallSign,
                                 Guid DefaultChannel,
                                 string Name,
                                 string RxFrequency,
                                 string TxFrequency,
                                 string Squelch,
                                 string TxCtcss,
                                 string RxCtCss,
                                 string Volume,
                                 string PreEmph,
                                 string HighPass,
                                 string LowPass,
                                 string SquelchDetection) : IRequest<Validation<Error,Guid>>;

    internal class InstallCommandHandler : IRequestHandler<InstallCommand, Validation<Error, Guid>>
    {
        private readonly InstallerService installerService;
        private readonly ILogger<InstallCommandHandler> logger;

        public InstallCommandHandler(InstallerService installerService, ILogger<InstallCommandHandler> logger)
        {
            this.installerService = installerService;
            this.logger = logger;
        }

        public Task<Validation<Error, Guid>> Handle(InstallCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Execution du handler de la commande InstallCommand");

               return installerService.InstallSvxlinkManager(
                                request.UserName,
                                request.Password,
                                request.ConfigId,
                                 request.InstallChannels,
                                 request.CallSign,
                                 request.AnnonceCallSign,
                                 request.DefaultChannel,
                                 request.Name,
                                 request.RxFrequency,
                                 request.TxFrequency,
                                 request.Squelch,
                                 request.TxCtcss,
                                 request.RxCtCss,
                                 request.Volume,
                                 request.PreEmph,
                                 request.HighPass,
                                 request.LowPass,
                                 request.SquelchDetection).AsTask();
            
        }

       

    }
}
