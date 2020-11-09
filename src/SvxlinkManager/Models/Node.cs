using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SvxlinkManager.Models
{
  public class Node : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    private string className = "node";

    public string Name { get; set; }

    public string ClassName
    {
      get => className; set
      {
        className = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ClassName)));
      }
    }

    public override bool Equals(object obj)=> Name == ((Node)obj).Name;
    
  }
}
