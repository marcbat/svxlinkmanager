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
    #region Fields

    private CancellationTokenSource cancelation;

    #endregion Fields

    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Properties

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Current Channel to Create or Edit
    /// </summary>
    /// <value>The channel.</value>
    protected Channel Channel { get; set; }

    /// <summary>
    /// Submit button label
    /// </summary>
    /// <value>The submit label.</value>
    protected abstract string SubmitTitle { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public virtual async Task HandleValidSubmit()
    {
      if (Channel.Sound == null)
        return;

      using var file = File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), "Sounds", Channel.Sound.Name));
      using var stream = Channel.Sound.OpenReadStream();

      var buffer = new byte[4 * 1096];
      int bytesRead;

      while ((bytesRead = await stream.ReadAsync(buffer, cancelation.Token)) != 0)
      {
        await file.WriteAsync(buffer, cancelation.Token);
      }
    }

    protected override void OnInitialized()
    {
      base.OnInitialized();

      cancelation = new CancellationTokenSource();
    }

    #endregion Methods
  }
}