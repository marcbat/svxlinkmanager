using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Spotnik.Gui.Models
{

  public class Channel
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string SvxLinkFile { get; set; }

    public string RestartFile { get; set; }
  }
}
