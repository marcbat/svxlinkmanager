using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.RadioProfile
{
  public class CreateBase : AddEditBase
  {
    protected override void OnInitialized()
    {
      RadioProfile = new Models.RadioProfile();
    }

    protected override string SubmitTitle => "Créer";

    protected override async Task HandleValidSubmitAsync()
    {
      Repositories.RadioProfiles.Add(RadioProfile);

      await ShowToastAsync("Crée", $"Le profil radio {RadioProfile.Name} a bien été crée.", ToastType.Success);

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }
  }
}