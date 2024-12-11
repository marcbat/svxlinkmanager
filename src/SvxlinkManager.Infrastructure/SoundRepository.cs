using LanguageExt;
using LanguageExt.Common;

using LiteDB;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SvxlinkManager.Application;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Infrastructure
{
    public class SoundRepository : ISoundRepository
    {
        private readonly ILogger<SoundRepository> logger;
        private SvxlinkManagerOptions options;

        public SoundRepository(IOptions<SvxlinkManagerOptions> options, ILogger<SoundRepository> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        public Validation<Error, Unit> CreateAsyc(Sound sound)
        {
            try
            {
                using var db = new LiteDatabase(options.LiteDbFile);

                var fs = db.FileStorage;

                var stream = new MemoryStream(sound.SoundFile);

                fs.Upload(sound.Id, sound.Name, stream);

                return Unit.Default;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la création du fichier son.");
                return Error.New("Erreur lors de la création du fichier son.");
            }

            

        }

        public Validation<Error, Unit> DeleteAsync(string name)
        {
            try
            {
                using var db = new LiteDatabase(options.LiteDbFile);

                var fs = db.FileStorage;

                fs.Delete($"$/sounds/{name}");

                return Unit.Default;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la suppression du fichier son.");
                return Error.New("Erreur lors de la suppression du fichier son.");
            }
        }

        public Validation<Error, Unit> UpdateAsyc(string name, Sound sound)
        {
            try
            {
               throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la mise à jour du fichier son.");
                return Error.New("Erreur lors de la mise à jour du fichier son.");
            }
        }
    }
}
