using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManagerTests
{
  public static class ProtectedExtensions
  {
    public static object Protected(this object target, string name, params object[] args)
    {
      var type = target.GetType();
      var method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                       .Where(x => x.Name == name && x.IsVirtual).Single();
      return method.Invoke(target, args);
    }
  }
}