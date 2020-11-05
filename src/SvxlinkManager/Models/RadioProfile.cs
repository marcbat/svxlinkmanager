using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class RadioProfile : IModelEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Enable { get; set; }

    [RegularExpression("^[0-9]{3}.[0-9]{3}")]
    public string RxFequ { get; set; }

    [RegularExpression("^[0-9]{3}.[0-9]{3}")]
    public string TxFrequ { get; set; }

    [Range(0,8)]
    public int Squelch { get; set; }

    public string TxCtcss { get; set; }

    public string RxCtcss { get; set; }
  }
}
