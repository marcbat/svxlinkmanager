using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class EditBase : AddEditBase
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
    public override async Task HandleValidSubmit()
    {
      Repositories.Channels.Update(Channel);

      await ShowSuccessToastAsync("Modifié", $"Le salon {Channel.Name} a bien été modifié.");

      NavigationManager.NavigateTo("Channel/Manage");
    }

    protected override void OnInitialized()
    {
      Channel = Repositories.Channels.Get(int.Parse(Id));
    }

    #endregion Methods
  }
}