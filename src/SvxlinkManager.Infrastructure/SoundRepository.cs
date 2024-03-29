using LiteDB;

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
        private SvxlinkManagerOptions options;

        public SoundRepository(IOptions<SvxlinkManagerOptions> options)
        {
            this.options = options.Value;
        }

        public async Task CreateAsyc(Sound sound)
        {
            using var db = new LiteDatabase(options.LiteDbFile);

            var collection = db.GetCollection<Sound>("sounds");

            collection.EnsureIndex(x => x.Id, true);

            collection.Insert(sound);
        }
    }
}
