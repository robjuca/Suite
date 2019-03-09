/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Message;

using Layout.Drawer.Shell.Presentation;
using Layout.Drawer.Shell.Pattern.Models;
//---------------------------//

namespace Layout.Drawer.Shell.Pattern.ViewModels
{
  [Export ("ShellCollectionViewModel", typeof (IShellCollectionViewModel))]
  public class TShellCollectionViewModel : TViewModelAware<TShellCollectionModel>, IHandleNavigateResponse, IShellCollectionViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellCollectionViewModel (IShellPresentation presentation)
      : base (new TShellCollectionModel ())
    {
      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TNavigateResponseMessage message)
    {
      if (message.IsActionNavigateTo) {
        if (message.IsSender (TNavigateMessage.TSender.Shell)) {
          if (message.IsWhere (TNavigateMessage.TWhere.Collection)) {
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