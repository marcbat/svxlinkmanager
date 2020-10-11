using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.Gui.ViewModels
{
  public class HomeBase : ComponentBase
  {
    private int salon;

    public int Salon { 
      get => salon; 
      set { 
        salon = value;

        RunRestart();
      } 
    }

    private void RunRestart()
    {
      // Start the child process.
      Process p = new Process();
      // Redirect the output stream of the child process.
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.FileName = "/etc/spotnik/restart.rrf";
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      p.WaitForExit();
    }

    public Dictionary<int, string> Salons => new Dictionary<int, string> { { 1, "Réseau des répéteurs francophones" }, { 2, "Salon suisse romande" } };

  }

}
