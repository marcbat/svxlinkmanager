using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class SvxlinkManagerParameter : IModelEntity
  {
    public int Id { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }
  }
}