/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Message;

using Module.Settings.Shell.Presentation;
using Module.Settings.Shell.Pattern.Models;
//---------------------------//

namespace Module.Settings.Shell.Pattern.ViewModels
{
  [Export ("ModuleShellFactoryDatabaseViewModel", typeof (IShellFactoryDatabaseViewModel))]
  public class TShellFactoryDatabaseViewModel : TViewModelAware<TShellFactoryDatabaseModel>, IHandleNavigateResponse, IShellFactoryDatabaseViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellFactoryDatabaseViewModel (IShellPresentation presentation)
      : base (new TShellFactoryDatabaseModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TNavigateResponseMessage message)
    {
      if (message.IsActionNavigateTo) {
        if (message.IsSender (TNavigateMessage.TSender.Shell)) {
          if (message.IsWhere (TNavigateMessage.TWhere.Database)) {
            ShowViewAnimation ();
          }

          else {
            HideViewAnimation ();
          }
        }
      }
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