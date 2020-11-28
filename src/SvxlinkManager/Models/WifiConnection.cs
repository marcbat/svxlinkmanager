using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class WifiConnection : IModelEntity
  {
    #region Properties

    public int Id { get; set; }

    public string Ssid { get; set; }

    public string Uuid { get; set; }

    public string Password { get; set; }

    public int Bars { get; set; }

    public bool InUse { get; set; }

    #endregion Properties
  }
}