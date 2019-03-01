/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Module.Settings.Shell.Presentation;
using Module.Settings.Shell.Pattern.Models;
//---------------------------//

namespace Module.Settings.Shell.Pattern.ViewModels
{
  [Export ("ModuleShellEditViewModel", typeof (IShellEditViewModel))]
  public class TShellEditViewModel : TViewModelAware<TShellEditModel>, IShellEditViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellEditViewModel (IShellPresentation presentation)
      : base (new TShellEditModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
    }
    #endregion

    #region Property
    IDelegateCommand DelegateCommand
    {
      get
      {
        return (PresentationCommand as IDelegateCommand);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace