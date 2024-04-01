using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public interface IModelEntity
  {
    public Guid Id { get; set; }
  }
}