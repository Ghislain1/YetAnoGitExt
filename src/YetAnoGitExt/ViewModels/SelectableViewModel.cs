
namespace YetAnoGitExt.ViewModels;

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SelectableViewModel : BindableBase
{
              private bool isSelected;
              private string? name;
              private string? description;
              private char code;
              private double numeric;
              private string? kind;
              private string? files;


              public bool IsSelected
              {
                            get => this.isSelected;
                            set => this.SetProperty(ref this.isSelected, value);
              }

              public char Code
              {
                            get => this.code;
                            set => this.SetProperty(ref this.code, value);
              }

              public string? Name
              {
                            get => this.name;
                            set => this.SetProperty(ref this.name, value);
              }

              public string? Description
              {
                            get => this.description;
                            set => this.SetProperty(ref this.description, value);
              }

              public double Numeric
              {
                            get => this.numeric;
                            set => this.SetProperty(ref this.numeric, value);
              }

              public string? Kind
              {
                            get => this.kind;
                            set => this.SetProperty(ref this.kind, value);
              }

              public string? Files
              {
                            get => this.files;
                            set => this.SetProperty(ref this.files, value);
              }


}
