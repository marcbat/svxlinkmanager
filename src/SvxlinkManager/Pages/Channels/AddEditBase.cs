using Microsoft.AspNetCore.Components;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public abstract class AddEditBase<TChannel> : RepositoryComponentBase where TChannel : ManagedChannel
  {
    private CancellationTokenSource cancelation;

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Current Channel to Create or Edit
    /// </summary>
    /// <value>The channel.</value>
    protected TChannel Channel { get; set; }

    /// <summary>
    /// Submit button label
    /// </summary>
    /// <value>The submit label.</value>
    protected abstract string SubmitTitle { get; }

    protected bool CanEditType => !Channel.IsDefault;

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    protected virtual async Task HandleValidSubmit()
    {
      if (Channel.SoundBrowserFile == null)
        return;

      using var stream = Channel.SoundBrowserFile.OpenReadStream();

      using (var memoryStream = new MemoryStream())
      {
        await stream.CopyToAsync(memoryStream);
        Channel.Sound.SoundFile = memoryStream.ToArray();
      }

      Channel.Sound.SoundName = Channel.SoundBrowserFile.Name;
    }

    protected override void OnInitialized()
    {
      base.OnInitialized();

      cancelation = new CancellationTokenSource();
    }
  }
}