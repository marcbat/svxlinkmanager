using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using SvxlinkManager.Models;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public class CreateBase : AddEditBase
  {
    #region Properties

    protected override string SubmitTitle => "Créer";

    #endregion Properties

    #region Methods

    /// <summary>
    /// Handles the form submit.
    /// </summary>
    public override async Task HandleValidSubmit()
    {
      await base.HandleValidSubmit();

      Repositories.Channels.Add(Channel);

      await ShowSuccessToastAsync("Success", $"Le salon {Channel.Name} a été crée.");

      NavigationManager.NavigateTo("Channel/Manage");
    }

    protected override void OnInitialized()
    {
      base.OnInitialized();

      Channel = new Channel();
    }

    #endregion Methods
  }
}