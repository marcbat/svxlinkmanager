using Microsoft.EntityFrameworkCore;

using SvxlinkManager.Data;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
    public interface IChannelRepository : IRepository<ManagedChannel>
    {
        ManagedChannel GetDefault();

        ManagedChannel GetWithSound(int id);
    }

    public class ChannelRepository : Repository<ManagedChannel>, IChannelRepository
    {
        public ChannelRepository(Data.IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public override ManagedChannel Add(ManagedChannel channel)
        {
            if (channel.IsDefault)
            {
                foreach (var c in GetAll())
                {
                    c.IsDefault = false;
                    Update(c);
                }
            }

            return base.Add(channel);
        }

        public override void Update(ManagedChannel channel)
        {
            base.Update(channel);

            // Set other channel to none default
            if (channel.IsDefault)
            {
                foreach (var c in GetAll().Where(c => !c.Id.Equals(channel.Id)))
                {
                    c.IsDefault = false;
                    Update(c);
                }
            }
        }

        public override void Delete(int id)
        {
            var channel = Get(id);
            if (channel.IsDefault)
                return;

            base.Delete(id);
        }

        public ManagedChannel GetDefault()
        {
            using var dbcontext = contextFactory.CreateDbContext();
            return dbcontext.Channels.SingleOrDefault(c => c.IsDefault);
        }

        public ManagedChannel GetWithSound(int id)
        {
            using var dbcontext = contextFactory.CreateDbContext();
            return dbcontext.Channels.Include(c => c.Sound).Single(e => e.Id == id);
        }
    }
}