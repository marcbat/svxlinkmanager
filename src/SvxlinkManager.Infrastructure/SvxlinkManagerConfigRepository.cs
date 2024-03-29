using LiteDB;

using Microsoft.Extensions.Options;

using SvxlinkManager.Application;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Infrastructure
{
    public class SvxlinkManagerConfigRepository : ISvxlinkManagerConfigRepository
    {
        private SvxlinkManagerOptions options;

        public SvxlinkManagerConfigRepository(IOptions<SvxlinkManagerOptions> options)
        {
            this.options = options.Value;
        }

        public Task Create(SvxlinkManagerConfigAggregate config)
        {
            using var db = new LiteDatabase(options.LiteDbFile);

            var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

            collection.EnsureIndex(x => x.Id, true);

            collection.Insert(config);

            return Task.CompletedTask;
        }

        public Task<SvxlinkManagerConfigAggregate> GetConfigAsync(Guid configId)
        {
            using var db = new LiteDatabase(options.LiteDbFile);

            var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

            collection.EnsureIndex(x => x.Id, true);

            return Task.FromResult(collection.FindById(configId));
        }

        public Task UpdateAsync(SvxlinkManagerConfigAggregate config)
        {
            using var db = new LiteDatabase(options.LiteDbFile);

            var collection = db.GetCollection<SvxlinkManagerConfigAggregate>("svxlinkManagerConfigs");

            collection.EnsureIndex(x => x.Id, true);

            collection.Update(config);

            return Task.CompletedTask;
        }
    }
    
}
