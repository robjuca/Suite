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
  [Export ("ModuleSQLADBFrontSettingsViewModel", typeof (ISQLADBFrontSettingsViewModel))]
  public class TSQLADBFrontSettingsViewModel : TViewModelAware<TSQLADBFrontSettingsModel>, IHandleMessageInternal, ISQLADBFrontSettingsViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TSQLADBFrontSettingsViewModel (IFactoryDatabasePresentation presentation)
      : base (new TSQLADBFrontSettingsModel ())
    {
      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region Handle
    public void Handle (TMessageInternal message)
    {
      // from parent
      if (message.Node.IsParentToMe (TChild.Front)) {
        if (message.IsAction (TInternalMessageAction.DatabaseValidated)) {
          Model.Populate (message.Support.Argument.Types.ConnectionData);
          RaiseChanged ();
        }
      }

      // from sibiling
      if (message.Node.IsSibilingToMe (TChild.Front)) {
        if (message.Support.Argument.Types.Authentication.Equals (TAuthentication.SQL)) {
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.Invoke (ResponseDataDispatcher);
          }

          if (message.IsAction (TInternalMessageAction.EditEnter)) {
            Model.FactoryEnter ();
            RaiseChanged ();
          }

          if (message.IsAction (TInternalMessageAction.EditLeave)) {
            Model.FactoryLeave ();
            RaiseChanged ();
          }

          if (message.IsAction (TInternalMessageAction.Change)) {
            Model.Populate (message.Support.Argument.Types.ConnectionData);
            RaiseChanged ();
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnFactoryCommandClicked ()
    {
      // to sibiling 
      var message = new TFactoryMessageInternal (TInternalMessageAction.EditEnter, TAuthentication.Windows, TypeInfo);
      message.Node.SelectRelationSibiling (TChild.None);

      DelegateCommand.PublishInternalMessage.Execute (message);
    } 
    #endregion

    #region Dispatcher
    void ResponseDataDispatcher ()
    {
      // to sibiling back
      var message = new TFactoryMessageInternal (TInternalMessageAction.Select, TAuthentication.SQL, TypeInfo);
      message.Node.SelectRelationSibiling (TChild.Front);
      message.Support.Argument.Types.ConnectionData.CopyFrom (Model.DatabaseAuthentication);

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