/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Message;

using Layout.Bag.Shell.Presentation;
using Layout.Bag.Shell.Pattern.Models;
//---------------------------//

namespace Layout.Bag.Shell.Pattern.ViewModels
{
  [Export ("ShellFactoryViewModel", typeof (IShellFactoryViewModel))]
  public class TShellFactoryViewModel : TViewModelAware<TShellFactoryModel>, IHandleNavigateResponse, IShellFactoryViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellFactoryViewModel (IShellPresentation presentation)
      : base (new TShellFactoryModel ())
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
          if (message.IsWhere (TNavigateMessage.TWhere.Factory)) {
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