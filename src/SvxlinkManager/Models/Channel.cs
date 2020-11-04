using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SvxlinkManager.Models
{

  public class Channel : IModelEntity
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Host { get; set; }

    public string AuthKey { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string CallSign { get; set; }

    [Required]
    public bool IsDefault { get; set; }

    [Required]
    public bool IsTemporized { get; set; }

    public int Dtmf { get; set; }
  }
}
