using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SvxlinkManager.Models
{
  public class Reflector : IModelEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Config { get; set; }

    public bool Enable { get; set; }

    [NotMapped]
    public Dictionary<string, string> TrackProperties => new Dictionary<string, string> {
        { nameof(Name), Name }
        };
  }
}