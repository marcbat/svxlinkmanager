using IniParser;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Service
{
  public interface IIniService
  {
    string FindConfigValueInFile(string filePath, string key);

    string FindConfigValueInString(string ini, string key);

    void ReplaceConfig(string filePath, Dictionary<string, Dictionary<string, string>> parameters);
  }

  public class IniService : IIniService
  {
    /// <summary>
    /// Replace parameters int svxlink.current ini file
    /// </summary>
    /// <param name="parameters">Dictionnary of parameters</param>
    /// <example>
    /// <para>
    /// var global = new Dictionary&lt;string, string&gt; <br/> { <br/> { "LOGICS",
    /// "SimplexLogic,ReflectorLogic" } <br/> }; <br/> var simplexlogic = new Dictionary&lt;string,
    /// string&gt; { <br/> { "MODULES", "ModuleHelp,ModuleMetarInfo,ModulePropagationMonitor"},
    /// <br/> { "CALLSIGN", channel.ReportCallSign}, <br/> { "REPORT_CTCSS", radioProfile.RxTone}
    /// <br/> }; <br/> var ReflectorLogic = new Dictionary&lt;string, string&gt; <br/> { <br/>
    /// {"CALLSIGN", channel.CallSign }, <br/> {"HOST", channel.Host }, <br/>
    /// {"AUTH_KEY",channel.AuthKey }, <br/> {"PORT" ,channel.Port.ToString()}
    /// </para>
    /// <para>
    /// }; <br/> var parameters = new Dictionary&lt;string, Dictionary&lt;string, string&gt;&gt;
    /// <br/> { <br/> {"GLOBAL", global }, <br/> {"SimplexLogic", simplexlogic }, <br/>
    /// {"ReflectorLogic" , ReflectorLogic} <br/> }; <br/>
    /// </para>
    /// <code></code>
    /// </example>
    public virtual void ReplaceConfig(string filePath, Dictionary<string, Dictionary<string, string>> parameters)
    {
      var parser = new FileIniDataParser();
      parser.Parser.Configuration.NewLineStr = "\r\n";
      parser.Parser.Configuration.AssigmentSpacer = string.Empty;

      var utf8WithoutBom = new UTF8Encoding(false);

      var data = parser.ReadFile(filePath, utf8WithoutBom);

      foreach (var section in parameters)
        foreach (var parameter in section.Value)
          data[section.Key][parameter.Key] = parameter.Value;

      parser.WriteFile(filePath, data, utf8WithoutBom);
    }

    public virtual string FindConfigValueInFile(string filePath, string key)
    {
      var parser = new FileIniDataParser();
      parser.Parser.Configuration.NewLineStr = "\r\n";
      parser.Parser.Configuration.AssigmentSpacer = string.Empty;

      var utf8WithoutBom = new UTF8Encoding(false);

      var data = parser.ReadFile(filePath, utf8WithoutBom);

      if (data.TryGetKey(key, out string value))
        return value;

      return null;
    }

    public virtual string FindConfigValueInString(string ini, string key)
    {
      var parser = new FileIniDataParser();
      parser.Parser.Configuration.NewLineStr = "\r\n";
      parser.Parser.Configuration.AssigmentSpacer = string.Empty;

      byte[] byteArray = Encoding.UTF8.GetBytes(ini);
      var stream = new MemoryStream(byteArray);
      var reader = new StreamReader(stream);

      var data = parser.ReadData(reader);

      if (data.TryGetKey(key, out string value))
        return value;

      return null;
    }
  }
}