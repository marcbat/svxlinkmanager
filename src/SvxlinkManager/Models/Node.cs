using System;
using System.Collections.Generic;
using System.Text;

namespace SvxlinkManager.Models
{
  public class Node
  {
    private string className = "node";

    public string Name { get; set; }

    public string ClassName
    {
      get => className;
      set => className = value;
    }

    public override bool Equals(object obj) => Name.Trim() == ((Node)obj).Name.Trim();
  }
}