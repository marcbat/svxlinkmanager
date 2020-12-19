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
  public abstract class AddEditBase<TChannel> : RepositoryComponentBase where TChannel : Channel
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

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    protected virtual async Task HandleValidSubmit()
    {
      if (Channel.Sound == null)
        return;

      using var file = File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), "Sounds", Channel.Sound.Name));
      using var stream = Channel.Sound.OpenReadStream();

      var buffer = new byte[4 * 1096];
      int bytesRead;

      while ((bytesRead = await stream.ReadAsync(buffer)) != 0)
        await file.WriteAsync(buffer);

      Channel.SoundName = Channel.Sound.Name;
    }

    protected override void OnInitialized()
    {
      base.OnInitialized();

      cancelation = new CancellationTokenSource();
    }
  }
}