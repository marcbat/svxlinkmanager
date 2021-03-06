﻿using Microsoft.JSInterop;

using SvxlinkManager.Pages.Channels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Pages.AdvanceChannel
{
  public class EditBase : EditBase<Models.AdvanceSvxlinkChannel>
  {
    public override async Task HandleValidSubmit(string redirect)
    {
      StateHasChanged();

      //var toto = await Js.InvokeAsync<string>("GetEditorValue", new object[] { "SvxlinkConfEditor" });

      //await Js.InvokeVoidAsync("EditorToTextArea");

      await base.HandleValidSubmit(redirect);
    }
  }
}