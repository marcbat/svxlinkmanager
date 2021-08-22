using NUnit.Framework;

using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service.Tests
{
  [TestFixture()]
  public class IniServiceTests
  {
    [Test()]
    public void FindConfigValueInStringTest()
    {
      var sb = new StringBuilder();
      sb.AppendLine("[GLOBAL]");
      sb.AppendLine("TIMESTAMP_FORMAT = \"%c\"");
      sb.AppendLine("LISTEN_PORT = 5300");
      sb.AppendLine("SQL_TIMEOUT = 600");
      sb.AppendLine("SQL_TIMEOUT_BLOCKTIME = 60");
      sb.AppendLine("CODECS = OPUS");
      sb.AppendLine(string.Empty);
      sb.AppendLine("[USERS]");
      sb.AppendLine("SM0ABC-1=MyNodes");
      sb.AppendLine("SM0ABC-2=MyNodes");
      sb.AppendLine("SM1XYZ=SM1XYZ");
      sb.AppendLine(string.Empty);
      sb.AppendLine("[PASSWORDS]");
      sb.AppendLine("MyNodes = \"A very strong password!\"");
      sb.AppendLine("SM1XYZ=\"Another very good password ? \"");

      var service = new IniService();

      var port = service.FindConfigValueInString(sb.ToString(), "GLOBAL.LISTEN_PORT");

      Assert.AreEqual(port, "5300");
    }

    [Test()]
    public void FindConfigValueInStringNullTest()
    {
      var sb = new StringBuilder();
      sb.AppendLine("[GLOBAL]");
      sb.AppendLine("TIMESTAMP_FORMAT = \"%c\"");
      sb.AppendLine("SQL_TIMEOUT = 600");
      sb.AppendLine("SQL_TIMEOUT_BLOCKTIME = 60");
      sb.AppendLine("CODECS = OPUS");
      sb.AppendLine(string.Empty);
      sb.AppendLine("[USERS]");
      sb.AppendLine("SM0ABC-1=MyNodes");
      sb.AppendLine("SM0ABC-2=MyNodes");
      sb.AppendLine("SM1XYZ=SM1XYZ");
      sb.AppendLine(string.Empty);
      sb.AppendLine("[PASSWORDS]");
      sb.AppendLine("MyNodes = \"A very strong password!\"");
      sb.AppendLine("SM1XYZ=\"Another very good password ? \"");

      var service = new IniService();

      var port = service.FindConfigValueInString(sb.ToString(), "GLOBAL.LISTEN_PORT");

      Assert.AreEqual(port, null);
    }
  }
}