using Microsoft.AspNetCore.Components;

using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class ManageBase : ChannelBase
  {
    #region Properties

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public void Delete(int id)
    {
      Repositories.Channels.Delete(id);

      Channels.Remove(Channels.Single(c => c.Id == id));

      StateHasChanged();

      NavigationManager.NavigateTo("/Channel/Manage");
    }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);
    }

    #endregion Methods
  }
}