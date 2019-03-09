/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Types;
using rr.Library.Message;
using rr.Library.Helper;

using Shared.Message;
using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Gadget.Image.Shell.Presentation;
using Gadget.Image.Shell.Pattern.Models;
//---------------------------//

namespace Gadget.Image.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TShellViewModel<TShellModel>
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (new TShellModel (), TProcess.IMAGE)
    {
      presentation.ViewModel = this;
    }
    #endregion

    #region Overrides
    public override void ProcessMessage (TMessageModule message)
    {
      // services
      if (message.IsModule (TResource.TModule.Services)) {
        // SettingsValidated
        if (message.IsAction (TMessageAction.SettingsValidated)) {
          SelectAuthentication (message.Support.Argument.Types.Authentication);

          // Success
          if (message.Support.IsActionStatus (TActionStatus.Success)) {
            TDispatcher.Invoke (DatabaseSettingsSuccessDispatcher);
          }

          // Error
          if (message.Support.IsActionStatus (TActionStatus.Error)) {
            TDispatcher.Invoke (DatabaseSettingsErrorDispatcher);
          }
        }
      }

      // focus
      if (message.IsAction (TMessageAction.Focus)) {
        if (message.Support.Argument.Args.IsWhere (TWhere.Collection)) {
          OnCollectionCommadClicked ();
        }

        if (message.Support.Argument.Args.IsWhere (TWhere.Factory)) {
          OnFactoryCommadClicked ();
        }
      }
    }
    #endregion

    #region Interface Members
    //public void Message (TMessageModule message)
    //{
    //  // services
    //  if (message.IsModule (TResource.TModule.Services)) {
    //    if (message.IsAction (TMessageAction.SettingsValidated)) {
    //      if (message.Support.IsActionStatus (TActionStatus.Success)) {
    //        TDispatcher.Invoke (DatabaseSettingsSuccessDispatcher);
    //      }

    //      if (message.Support.IsActionStatus (TActionStatus.Error)) {
    //        TDispatcher.Invoke (DatabaseSettingsErrorDispatcher);
    //      }
    //    }
    //  }

    //  // error
    //  if (message.IsAction (TMessageAction.Error)) {
    //    TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, message.Support.ErrorMessage);
    //  }

    //  // focus
    //  if (message.IsAction (TMessageAction.Focus)) {
    //    if (message.Support.Argument.Args.IsWhere (TWhere.Collection)) {
    //      OnCollectionCommadClicked ();
    //    }

    //    if (message.Support.Argument.Args.IsWhere (TWhere.Factory)) {
    //      OnFactoryCommadClicked ();
    //    }
    //  }

    //  // modal enter
    //  if (message.IsAction (TMessageAction.ModalEnter)) {
    //    if (m_ModalCount == 0) {
    //      Model.Lock ();
    //      Model.ShowPanels ();

    //      RaiseChanged ();
    //    }

    //    m_ModalCount++;
    //  }

    //  // modal leave
    //  if (message.IsAction (TMessageAction.ModalLeave)) {
    //    if (m_ModalCount > 0) {
    //      m_ModalCount--;

    //      if (m_ModalCount == 0) {
    //        Model.Unlock ();
    //        Model.ClearPanels ();

    //        RaiseChanged ();
    //      }
    //    }
    //  }

    //  // edit enter
    //  if (message.IsAction (TMessageAction.EditEnter)) {
    //    Model.EditLock ();
    //    RaiseChanged ();
    //  }

    //  // edit leave
    //  if (message.IsAction (TMessageAction.EditLeave)) {
    //    if (m_ModalCount == 0) {
    //      Model.EditUnlock ();
    //      RaiseChanged ();
    //    }
    //  }
    //}
    #endregion

    #region View Event
    public void OnCollectionCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Collection));

      RaiseChanged ();
    }

    public void OnFactoryCommadClicked ()
    {
      DelegateCommand.NotifyNavigateRequestMessage.Execute (new TNavigateRequestMessage (TNavigateMessage.TSender.Shell, TNavigateMessage.TWhere.Factory));

      RaiseChanged ();
    }
    #endregion

    #region Dispatcher
    void RequestServiceValidationDispatcher ()
    {
      // settings validating
      var message = new TShellMessage (TMessageAction.SettingsValidating, TypeInfo);
      DelegateCommand.PublishModuleMessage.Execute (message);
    }

    void DatabaseSettingsSuccessDispatcher ()
    {
      Model.ClearPanels ();
      Model.DatabaseStatus (true);
      Model.Unlock ();

      RaiseChanged ();

      OnCollectionCommadClicked ();

      // notify modules
      var message = new TShellMessage (TMessageAction.DatabaseValidated, TypeInfo);
      DelegateCommand.PublishModuleMessage.Execute (message);
    }

    void DatabaseSettingsErrorDispatcher ()
    {
      Model.ClearPanels ();
      Model.DatabaseStatus (false);
      Model.Lock ();
      
      RaiseChanged ();
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      TDispatcher.Invoke (RequestServiceValidationDispatcher);
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