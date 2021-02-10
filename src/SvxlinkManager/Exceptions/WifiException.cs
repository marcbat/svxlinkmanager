using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Exceptions
{
  public class WifiException : Exception
  {
    public WifiException()
    {
    }

    public WifiException(string message)
        : base(message)
    {
    }

    public WifiException(string message, Exception inner)
        : base(message, inner)
    {
    }
  }
}