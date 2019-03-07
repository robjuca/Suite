/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Message;
using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;

using Module.Settings.Shell.Presentation;
using Module.Settings.Shell.Pattern.Models;
//---------------------------//

namespace Module.Settings.Shell.Pattern.ViewModels
{
  [Export ("ModuleShellReportViewModel", typeof (IShellReportViewModel))]
  public class TShellReportViewModel : TViewModelAware<TShellReportModel>, IHandleNavigateResponse, IHandleMessageModule, IShellReportViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellReportViewModel (IShellPresentation presentation)
      : base (new TShellReportModel ())
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
          if (message.IsWhere (TNavigateMessage.TWhere.Report)) {
            ShowViewAnimation ();

            TDispatcher.Invoke (RefreshDispatcher);
          }

          else {
            HideViewAnimation ();
          }
        }
      }
    }

    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        if (message.IsAction (TMessageAction.Response)) {
          Model.Select (message.Support.Argument.Types.ConnectionData);
        }

        if (message.IsAction (TMessageAction.DatabaseValidated)) {
          if (message.Support.Argument.Types.EntityAction.Param1 is TComponentModelItem item) {
            Model.Select (item);
          }
        }
      }
    }
    #endregion

    #region Dispatcher
    void RefreshDispatcher ()
    {
      Model.Refresh ();
      RaiseChanged ();

      RefreshCollection ("PropertyInfoViewSource");
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