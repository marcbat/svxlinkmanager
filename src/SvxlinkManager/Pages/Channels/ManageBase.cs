﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using SvxlinkManager.Models;
using SvxlinkManager.Pages.Shared;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class ManageBase<TChannel> : RepositoryComponentBase where TChannel : Channel
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadChannels();
    }

    public List<TChannel> Channels { get; set; }

    private void LoadChannels() => Channels = Repositories.Repository<TChannel>().GetAll().ToList();

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    public async Task DeleteAsync(TChannel channel)
    {
      Repositories.Channels.Delete(channel.Id);

      Channels.Remove(channel);

      StateHasChanged();

      await ShowSuccessToastAsync("Supprimé", "Le salon a bien été supprimé.");
    }
  }
}