using SvxlinkManager.Data;
using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Repositories
{
    public interface IRadioProfileRepository : IRepository<RadioProfile>
    {
        RadioProfile GetCurrent();
    }

    public class RadioProfileRepository : Repository<RadioProfile>, IRadioProfileRepository
    {
        public RadioProfileRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public override void Update(RadioProfile profile)
        {
            base.Update(profile);

            if (profile.Enable)
            {
                foreach (var p in GetAll().Where(p => !p.Id.Equals(profile.Id)))
                {
                    p.Enable = false;
                    Update(p);
                }
            }
        }

        public override void Delete(int id)
        {
            var radioProfile = Get(id);
            if (radioProfile.Enable)
                return;

            base.Delete(id);
        }

        public RadioProfile GetCurrent()
        {
            using var dbcontext = contextFactory.CreateDbContext();
            return dbcontext.RadioProfiles.Single(p => p.Enable);
        }
    }
}