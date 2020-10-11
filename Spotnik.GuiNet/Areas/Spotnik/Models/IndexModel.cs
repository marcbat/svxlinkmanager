using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotnik.GuiNet.Areas.Spotnik.Models
{
  public class IndexModel : PageModel
  {

    public void OnGet()
    {
      Essai = "Test3";

      Salons = new List<SelectListItem> {
        new SelectListItem { Value = "1", Text = "Mike" },
        new SelectListItem { Value = "2", Text = "Pete" },
        new SelectListItem { Value = "3", Text = "Katy" },
        new SelectListItem { Value = "4", Text = "Carl" }
    };
    }

    public string Essai { get; set; }


    [BindProperty]
    public int Salon { get; set; } = 1;

    public List<SelectListItem> Salons { get; set; }

  }

  
}
