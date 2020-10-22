using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Spotnik.Gui.Models
{

  public class Channel : IModelEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Host { get; set; }

    public string AuthKey { get; set; }

    public int Port { get; set; }

    public string CallSign { get; set; }
  }
}
