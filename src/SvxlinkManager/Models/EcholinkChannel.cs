using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class EcholinkChannel : Channel
  {
    [Required]
    public string Password { get; set; }

    [Required]
    public string SysopName { get; set; }

    [Required]
    public string Location { get; set; }

    [Required]
    public int MaxQso { get; set; } = 1;

    [Required]
    public string Description { get; set; }
  }
}