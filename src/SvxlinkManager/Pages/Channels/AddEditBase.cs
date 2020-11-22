using Microsoft.AspNetCore.Components;
using SvxlinkManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public abstract class AddEditBase : RepositoryComponentBase, INotifyPropertyChanged
  {
    private CancellationTokenSource cancelation;

    protected override void OnInitialized()
    {
      base.OnInitialized();

      cancelation = new CancellationTokenSource();
    }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected Channel Channel { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected abstract string SubmitTitle { get; }

    public virtual async Task HandleValidSubmit()
    {
      using var file = File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), "Sounds", Channel.Sound.Name));
      using var stream = Channel.Sound.OpenReadStream();

      var buffer = new byte[4 * 1096];
      int bytesRead;

      while ((bytesRead = await stream.ReadAsync(buffer, cancelation.Token)) != 0)
      {
        await file.WriteAsync(buffer, cancelation.Token);
      }
    }
  }
}
