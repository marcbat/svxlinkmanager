using FileHelpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  [FixedLengthRecord(FixedMode.AllowVariableLength)]
  [IgnoreFirst]
  public class Connection
  {
    [FieldFixedLength(21)]
    [FieldTrim(TrimMode.Both)]
    public string Name { get; set; }

    [FieldFixedLength(38)]
    [FieldTrim(TrimMode.Both)]
    public string Uuid { get; set; }

    [FieldFixedLength(10)]
    [FieldTrim(TrimMode.Both)]
    public string Type { get; set; }

    [FieldFixedLength(6)]
    [FieldTrim(TrimMode.Both)]
    public string Device { get; set; }

    [FieldHidden]
    public string Password { get; set; }
  }
}