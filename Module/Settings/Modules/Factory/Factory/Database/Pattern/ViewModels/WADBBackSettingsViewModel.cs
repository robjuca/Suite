/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;

using Module.Settings.Factory.Database.Presentation;
using Module.Settings.Factory.Database.Pattern.Models;
//---------------------------//

namespace Module.Settings.Factory.Database.Pattern.ViewModels
{
  [Export ("ModuleWADBBackSettingsViewModel", typeof (IWADBBackSettingsViewModel))]
  public class TWADBBackSettingsViewModel : TViewModelAware<TWADBBackSettingsModel>, IHandleMessageInternal, IWADBBackSettingsViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TWADBBackSettingsViewModel (IFactoryDatabasePresentation presentation)
      : base (new TWADBBackSettingsModel ())
    {
      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region Handle
    public void Handle (TMessageInternal message)
    {
      // from sibiling
      if (message.Node.IsSiblingToMe (TChild.Back)) {
        if (message.Support.Argument.Types.Authentication.Equals (TAuthentication.Windows)) {
          if (message.IsAction (TInternalMessageAction.Select)) {
            Model.ClearCheck ();
            Model.Populate (message.Support.Argument.Types.ConnectionData);
            RaiseChanged ();
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnDatabaseApplyCommadClicked ()
    {
      Model.Apply ();
      RaiseChanged ();

      // to sibiling front
      var message = new TFactoryMessageInternal (TInternalMessageAction.Change, TAuthentication.Windows, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Back);
      message.Support.Argument.Types.ConnectionData.CopyFrom (Model.DatabaseConnectionData);

      DelegateCommand.PublishInternalMessage.Execute (message);

      TDispatcher.Invoke (NotifyChageDispatcher);
    }

    public void OnBackCommadClicked ()
    {
      Model.ClearCheck ();
      RaiseChanged ();

      // to sibiling 
      var message = new TFactoryMessageInternal (TInternalMessageAction.EditLeave, TAuthentication.Windows, TypeInfo);
      message.Node.SelectRelationSibling (TChild.None);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Dispatcher
    void NotifyChageDispatcher ()
    {
      // notify parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Change, TypeInfo);
      message.Node.SelectRelationChild (TChild.Back);
      message.Support.Argument.Types.ConnectionData.CopyFrom (Model.DatabaseConnectionData);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      // to sibiling
      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TAuthentication.Windows, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Back);

      DelegateCommand.PublishInternalMessage.Execute (message);
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