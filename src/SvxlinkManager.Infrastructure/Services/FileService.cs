using Microsoft.Extensions.Logging;

using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string applicationPath = Directory.GetCurrentDirectory();
        private readonly ILogger<FileService> logger;

        public FileService(ILogger<FileService> logger)
        {
            this.logger = logger;
        }

        public void WriteReflectorConfig(Reflector reflector)
        {
            File.WriteAllText($"{applicationPath}/SvxlinkConfig/svxreflector-{reflector.Id}.conf", reflector.Config);
            logger.LogInformation("Le fichier de configuration du reflector a été écrit avec succès.");
        }
    }
}
