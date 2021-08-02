using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using SvxlinkManager.Pages.Shared;
using SvxlinkManager.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Reflector
{
  [Authorize]
  public class ManageBase : RepositoryComponentBase
  {
    protected override async Task OnInitializedAsync()
    {
      Telemetry.TrackPageView(new PageViewTelemetry("Reflector Manage Page") { Url = new Uri("/Reflector/Manage", UriKind.Relative) });

      await base.OnInitializedAsync().ConfigureAwait(false);

      LoadReflectors();
    }

    private void LoadReflectors() => Reflectors = Repositories.Repository<Models.Reflector>().GetAll().ToList();

    [Inject]
    public SvxLinkService SvxLinkService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    [Inject]
    public ISa818Service Sa818Service { get; set; }

    public List<Models.Reflector> Reflectors { get; set; }

    public async Task DeleteAsync(int id)
    {
      Repositories.Repository<Models.Reflector>().Delete(id);

      Telemetry.TrackEvent("Delete reflector profile", Reflectors.Single(c => c.Id == id).TrackProperties);

      Reflectors.Remove(Reflectors.Single(c => c.Id == id));

      await ShowSuccessToastAsync("Supprimé", "le reflecteur a bien été supprimé.");

      StateHasChanged();
    }

    public async Task Start(int id)
    {
      var reflector = Repositories.Repository<Models.Reflector>().Get(id);

      reflector.Enable = true;
      Repositories.Repository<Models.Reflector>().Update(reflector);

      Telemetry.TrackEvent("Start reflector", reflector.TrackProperties);

      await ShowSuccessToastAsync($"{reflector.Name} démarré.", $"Le reflecteur {reflector.Name} a bien été démarré.");

      NavigationManager.NavigateTo("/Reflector/Manage", true);
    }

    public async Task Stop(int id)
    {
      var reflector = Repositories.Repository<Models.Reflector>().Get(id);

      reflector.Enable = false;
      Repositories.Repository<Models.Reflector>().Update(reflector);

      Telemetry.TrackEvent("Stop reflector", reflector.TrackProperties);

      await ShowSuccessToastAsync($"{reflector.Name} démarré.", $"Le reflecteur {reflector.Name} a bien été arreté.");

      NavigationManager.NavigateTo("/Reflector/Manage", true);
    }
  }
}