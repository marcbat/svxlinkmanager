using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Spotnik.Gui.Service
{
  public class SwxLinkLogService
  {
    public event Action<List<string>> Connected;

    public event Action<string> NodeConnected;
    public event Action<string> NodeDisconnected;

    public event Action<string> NodeTx;
    public event Action<string> NodeRx;

    private bool read = true;

    public async Task StartReadAsync()
    {
      read = true;

      var wh = new AutoResetEvent(false);
      var fsw = new FileSystemWatcher(".")
      {
        Filter = "file-to-read",
        EnableRaisingEvents = true
      };
      fsw.Changed += (s, e) => wh.Set();

      var fs = new FileStream("/tmp/svxlink.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      using (var sr = new StreamReader(fs))
      {
        var s = "";
        while (read)
        {
          s = await sr.ReadLineAsync();
          if (s != null)
            ParseLog(s);
          else
            wh.WaitOne(200);
        }
      }

      wh.Close();

    }

    public void StopRead() => read = false;

    private void ParseLog(string s)
    {
      if (s.Contains("Connected nodes"))
        Connected?.Invoke(s.Split(':')[5].Split(',').ToList());

      if (s.Contains("Node left"))
        NodeDisconnected?.Invoke(s.Split(":")[5]);

      if (s.Contains("Node joined"))
        NodeConnected?.Invoke(s.Split(":")[5]);

      if (s.Contains("Talker start"))
        NodeTx?.Invoke(s.Split(":")[5]);

      if (s.Contains("Talker stop"))
        NodeRx?.Invoke(s.Split(":")[5]);
    }
  }
}
