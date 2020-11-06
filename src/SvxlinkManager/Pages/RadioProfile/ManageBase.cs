using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public class ManageBase : RepositoryComponentBase
  {

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadRadioProfiles();
    }

    private void LoadRadioProfiles() => RadioProfiles = Repositories.RadioProfiles.GetAll().ToList();

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public List<Models.RadioProfile> RadioProfiles { get; set; }

    public void Delete(int id)
    {
      Repositories.RadioProfiles.Delete(id);

      RadioProfiles.Remove(RadioProfiles.Single(c => c.Id == id));

      StateHasChanged();

      //NavigationManager.NavigateTo("/RadioProfile/Manage");
    }

    public void Apply(int id)
    {
      var profile = Repositories.RadioProfiles.Get(id);

      var cmd = $"818cli-prog -g {profile.RxFequ}0,{profile.TxFrequ}0,{profile.RxCtcss},{profile.Squelch},{profile.TxCtcss}";

      Logger.LogInformation($"Application du profil {profile.Name}.");
      Logger.LogDebug($"Commande: {cmd}");

      //var escapedArgs = cmd.Replace("\"", "\\\"");

      //using var shell = new Process
      //{
      //  StartInfo = new ProcessStartInfo
      //  {
      //    FileName = "/bin/bash",
      //    Arguments = $"-c \"{escapedArgs}\"",
      //    RedirectStandardOutput = true,
      //    StandardOutputEncoding = Encoding.UTF8,
      //    RedirectStandardError = true,
      //    StandardErrorEncoding = Encoding.UTF8,
      //    UseShellExecute = false,
      //    CreateNoWindow = true,
      //  },

      //  EnableRaisingEvents = true
      //};

      //shell.ErrorDataReceived += new DataReceivedEventHandler(ShellErrorDataReceived);
      //shell.OutputDataReceived += new DataReceivedEventHandler(ShellOutputDataReceived);

      //shell.Start();
      //shell.BeginErrorReadLine();
      //shell.BeginOutputReadLine();

      //shell.WaitForExit(3000);

      profile.Enable = true;
      Repositories.RadioProfiles.Update(profile);

      NavigationManager.NavigateTo("/RadioProfile/Manage", true);
    }

    private void ShellOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      Logger.LogInformation(e.Data);
    }

    private void ShellErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      Logger.LogInformation(e.Data);
    }
  }
}
