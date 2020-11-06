using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class RadioProfile : IModelEntity, INotifyPropertyChanged
  {
    private bool enable;

    public int Id { get; set; }

    public string Name { get; set; }

    public bool Enable { 
      get => enable;
      set {
        enable = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Enable)));
      } 
    }

    [RegularExpression("^[0-9]{3}.[0-9]{3}")]
    public string RxFequ { get; set; }

    [RegularExpression("^[0-9]{3}.[0-9]{3}")]
    public string TxFrequ { get; set; }

    public string Squelch { get; set; } = "2";

    public string TxCtcss { get; set; } = "0000";

    public string RxCtcss { get; set; } = "0000";

    [NotMapped]
    public string TxTone { get => Ctcss.Single(c => c.Key == TxCtcss).Value; }

    [NotMapped]
    public string RxTone { get => Ctcss.Single(c => c.Key == RxCtcss).Value; }

    [NotMapped]
    public Dictionary<string, string> Ctcss => new Dictionary<string, string>
    {
      {"0000", "Pas de tone" },
      {"0001","67"},
      {"0002","71.9"},
      {"0003","74.4"},
      {"0004","77"},
      {"0005","79.7"},
      {"0006","82.5"},
      {"0007","85.4"},
      {"0008","88.5"},
      {"0009","91.5"},
      {"0010","94.8"},
      {"0011","97.4"},
      {"0012","100"},
      {"0013","103.5"},
      {"0014","107.2"},
      {"0015","110.9"},
      {"0016","114.8"},
      {"0017","118.8"},
      {"0018","123"},
      {"0019","127.3"},
      {"0020","131.8"},
      {"0021","136.5"},
      {"0022","141.3"},
      {"0023","146.2"},
      {"0024","151.4"},
      {"0025","156.7"},
      {"0026","162.2"},
      {"0027","167.9"},
      {"0028","173.8"},
      {"0029","179.9"},
      {"0030","186.2"},
      {"0031","192.8"},
      {"0032","203.5"},
      {"0033","210.7"},
      {"0034","218.1"},
      {"0035","225.7"},
      {"0036","233.6"},
      {"0037","241.8"},
      {"0038","250.3"}
    };

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
