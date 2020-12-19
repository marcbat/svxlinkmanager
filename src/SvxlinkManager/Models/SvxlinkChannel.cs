using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class SvxlinkChannel : Channel
  {
    public string AuthKey { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string ReportCallSign { get; set; }
  }
}