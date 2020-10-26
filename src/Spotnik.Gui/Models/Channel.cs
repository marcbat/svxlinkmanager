using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Spotnik.Gui.Models
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
  }
}
