using LiteDB;

using Microsoft.Extensions.Options;

using SvxlinkManager.Application;
using SvxlinkManager.Application.Interfaces;
using SvxlinkManager.Domain.Aggregates;
using SvxlinkManager.Domain.Entities;

using System.Net;
using System.Threading.Channels;

namespace SvxlinkManager.Infrastructure
{
  public class SvxlinkManagerConfigRepository : ISvxlinkManagerConfigRepository
  {
    private readonly SvxlinkManagerOptions options;

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

    public Task<IEnumerable<SvxlinkChannel>> GetAllOriginalChannels()
    {
      var channels = new List<SvxlinkChannel>
      {
        new(Guid.Parse("235a4521-15a1-4e02-a540-91ee600452ac"), "Réseau des Répéteurs Francophones", Guid.NewGuid(),"rrf2.f5nlg.ovh", 5300, "Fake", "Fake"),
        new(Guid.Parse("1f2e87b8-d984-4c05-8a4a-ffad65c829a9"), "Salon Suisse Romand", Guid.NewGuid(),"salonsuisseromand.northeurope.cloudapp.azure.com", 5300, "Fake", "Fake"),
        new(Guid.Parse("0f669a03-dcf1-4277-9b07-54f6a0fd3037"), "French Open Network", Guid.NewGuid(),"serveur.f1tzo.com", 5300, "Fake", "Fake"),
        new(Guid.Parse("a749ffe5-16c7-45da-809d-c048908f115c"), "Salon Technique", Guid.NewGuid(),"rrf3.f5nlg.ovh", 5301, "Fake", "Fake"),
        new(Guid.Parse("dd03fd9e-aeed-457e-97bf-973837a5fcec"), "Salon International", Guid.NewGuid(),"rrf3.f5nlg.ovh", 5302, "Fake", "Fake"),
        new(Guid.Parse("d4c59d86-947c-4b1d-831a-807c1877d426"), "Salon Bavardage", Guid.NewGuid(),"serveur.f1tzo.com", 5301, "Fake", "Fake"),
        new(Guid.Parse("9f99b18b-96ea-453d-b07a-7923c09c939f"), "Salon Local", Guid.NewGuid(),"serveur.f1tzo.com", 5302, "Fake", "Fake"),
        new(Guid.Parse("dcc5afa2-790f-40ca-b24d-bf91e90b1ac7"), "Salon Expérimental", Guid.NewGuid(),"rrf3.f5nlg.ovh", 5303, "Fake", "Fake"),
      };

      return Task.FromResult(channels.AsEnumerable());
    }
  }

}
