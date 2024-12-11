
using LanguageExt.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Commands;
using SvxlinkManager.Application.SvxlinkManagerConfigs.RadioProfils.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  [Authorize]
  public class EditBase : AddEditBase
  {
        protected override async void OnInitialized()
        {
            var result = await Mediatr.Send(new GetRadioProfilByIdQuery(Options.Value.ConfigId, Guid.Parse(Id)));

            result.Match(
                Fail: async error =>
                {
                    await ShowErrorToastAsync("Erreur", error.ToFullString());
                },
                Succ: success => RadioProfile = success
            );
        }

    [Parameter]
    public string Id { get; set; }

    override protected async Task HandleValidSubmitAsync()
    {
      await Mediatr.Send(new UpdateRadioProfilCommand(Options.Value.ConfigId, RadioProfile.Id, RadioProfile.Name, RadioProfile.RxFequ, RadioProfile.TxFrequ, RadioProfile.Squelch, RadioProfile.TxTone, RadioProfile.RxTone, RadioProfile.Volume, RadioProfile.PreEmph, RadioProfile.HightPass, RadioProfile.LowPass, RadioProfile.SquelchDetection));

      await ShowSuccessToastAsync("Modifié", $"le profil radio {RadioProfile.Name} a bien été modifié.");

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }

    protected override string SubmitTitle => "Modifier";
  }
}