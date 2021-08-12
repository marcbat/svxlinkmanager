using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class Rule : IModelEntity
  {
    public int Id { get; set; }

    public bool Enable { get; set; }

    [Required]
    public int Day { get; set; }

    [Required]
    public int Duration { get; set; }

    [Required]
    public Channel Channel { get; set; }
  }
}