using FileHelpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  [FixedLengthRecord(FixedMode.AllowLessChars)]
  [IgnoreFirst]
  public class Device
  {
    [FieldFixedLength(8)]
    [FieldTrim(TrimMode.Both)]
    public string InUse { get; set; }

    [FieldFixedLength(29)]
    [FieldTrim(TrimMode.Both)]
    public string Ssid { get; set; }

    [FieldFixedLength(7)]
    [FieldTrim(TrimMode.Both)]
    public string Mode { get; set; }

    [FieldFixedLength(6)]
    [FieldTrim(TrimMode.Both)]
    public string Channel { get; set; }

    [FieldFixedLength(10)]
    [FieldTrim(TrimMode.Both)]
    public string Rate { get; set; }

    [FieldFixedLength(8)]
    [FieldTrim(TrimMode.Both)]
    public string Signal { get; set; }

    [FieldFixedLength(6)]
    [FieldTrim(TrimMode.Both)]
    public string Bars { get; set; }

    [FieldFixedLength(10)]
    [FieldTrim(TrimMode.Both)]
    public string Security { get; set; }

    [FieldHidden]
    public string Password { get; set; }

    [FieldHidden]
    public bool HasConnection { get; set; }
  }
}