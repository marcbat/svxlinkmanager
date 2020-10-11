using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Spotnik.Gui.Models
{

  [XmlRoot("Channels")]
  public class ChannelsConfig
  {
    [XmlElement("Channel")]
    public List<Channel> Channels { get; set; }
  }

  public class Channel
  {
    public string Id { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("SvxLinkFile")]
    public string SvxLinkFile { get; set; }

    [XmlElement("RestartFile")]
    public string RestartFile { get; set; }
  }
}
