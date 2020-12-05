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

      await ShowSuccessToastAsync("Crée", $"Le profil radio {RadioProfile.Name} a bien été crée.");

      NavigationManager.NavigateTo("RadioProfile/Manage");
    }
  }
}