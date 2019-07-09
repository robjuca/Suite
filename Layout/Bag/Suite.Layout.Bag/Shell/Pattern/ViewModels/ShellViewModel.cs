/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Message;
using rr.Library.Helper;

using Shared.Message;
using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Layout.Bag.Shell.Presentation;
using Layout.Bag.Shell.Pattern.Models;
//---------------------------//

namespace Layout.Bag.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TShellViewModel<TShellModel>
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (new TShellModel (), TProcess.BAG)
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

          // sucess
          if (message.Support.IsActionStatus (TActionStatus.Success)) {
            TDispatcher.Invoke (DatabaseSettingsSuccessDispatcher);
          }

          // error
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

    public override void RefreshProcess ()
    {
      // notify modules
      var message = new TShellMessage (TMessageAction.RefreshProcess, TypeInfo);
      DelegateCommand.PublishModuleMessage.Execute (message);
    }
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