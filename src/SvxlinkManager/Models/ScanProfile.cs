using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class ScanProfile : IModelEntity
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int ScanDelay { get; set; }

    public List<Channel> Channels { get; set; } = new List<Channel>();

    public bool Enable { get; set; }
  }
}