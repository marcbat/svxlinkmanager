using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotnik.Gui.Service
{
  public class SvxLinkService
  {
    public event Action<List<string>> Connected;

    public event Action<string> NodeConnected;
    public event Action<string> NodeDisconnected;

    public event Action<string> NodeTx;
    public event Action<string> NodeRx;

    private readonly ILogger<SvxLinkService> logger;

    public SvxLinkService(ILogger<SvxLinkService> logger)
    {
      this.logger = logger;
    }

    public void RunsvxLink()
    {
      var cmd = "svxlink --pidfile=/var/run/svxlink.pid --runasuser=root --config=/etc/spotnik/svxlink.current";

      var escapedArgs = cmd.Replace("\"", "\\\"");

      var shell = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          StandardOutputEncoding = Encoding.UTF8,
          RedirectStandardError = true,
          StandardErrorEncoding = Encoding.UTF8,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      shell.EnableRaisingEvents = true;
      shell.ErrorDataReceived += new DataReceivedEventHandler(ShellErrorDataReceived);
      shell.OutputDataReceived += new DataReceivedEventHandler(ShellOutputDataReceived);

      shell.Start();
      shell.BeginErrorReadLine();
      shell.BeginOutputReadLine();
      //shell.WaitForExit();

    }

    private void ShellOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      logger.LogInformation(e.Data);
      ParseLog(e.Data);
    }

    private void ShellErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      logger.LogInformation(e.Data);
      ParseLog(e.Data);
    }

    private void ParseLog(string s)
    {
      if (string.IsNullOrEmpty(s))
        return;

      if (s.Contains("Connected nodes"))
        Connected?.Invoke(s.Split(':')[2].Split(',').ToList());

      if (s.Contains("Node left"))
        NodeDisconnected?.Invoke(s.Split(":")[2]);

      if (s.Contains("Node joined"))
        NodeConnected?.Invoke(s.Split(":")[2]);

      if (s.Contains("Talker start"))
        NodeTx?.Invoke(s.Split(":")[2]);

      if (s.Contains("Talker stop"))
        NodeRx?.Invoke(s.Split(":")[2]);
    }

    public void StopSvxlink()
    {
      var pid = ExecuteCommand("pgrep -x svxlink");
      if (pid != null)
        ExecuteCommand("pkill -TERM svxlink");
    }

    private string ExecuteCommand(string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");

      var process = new Process()
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
      process.Start();
      string result = process.StandardOutput.ReadToEnd();
      process.WaitForExit();

      return result.Trim();
    }
  }
}
