using Microsoft.AspNetCore.Components;
using SvxlinkManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.Channels
{
  public abstract class AddEditBase : RepositoryComponentBase, INotifyPropertyChanged
  {
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected Channel Channel { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected abstract void HandleValidSubmit();

    protected abstract string SubmitTitle { get; }
  }
}
