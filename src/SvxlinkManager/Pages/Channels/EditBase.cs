﻿using Microsoft.AspNetCore.Components;

using SvxlinkManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class EditBase<TChannel> : AddEditBase<TChannel> where TChannel : Channel
  {
    #region Properties

    [Parameter]
    public string Id { get; set; }

    protected override string SubmitTitle => "Modifier";

    #endregion Properties

    #region Methods

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public async Task HandleValidSubmit(string redirect)
    {
      Repositories.Repository<TChannel>().Update(Channel);

      await ShowSuccessToastAsync("Modifié", $"Le salon {Channel.Name} a bien été modifié.");

      NavigationManager.NavigateTo($"{redirect}/Manage");
    }

    protected override void OnInitialized()
    {
      Channel = Repositories.Repository<TChannel>().Get(int.Parse(Id));
    }

    #endregion Methods
  }
}