using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Wifi
{
  public class ManageBase : RepositoryComponentBase
  {
    #region Properties

    public List<WifiConnection> Connections { get; set; }

    #endregion Properties
  }
}