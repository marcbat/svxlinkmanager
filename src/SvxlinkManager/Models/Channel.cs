using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Common.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SvxlinkManager.Models
{
  public abstract class Channel : ManagedChannel
  {
    [Required]
    public string Host { get; set; }

    [Required]
    public string CallSign { get; set; }

    public List<ScanProfile> ScanProfiles { get; set; }
  }
}