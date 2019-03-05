/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Types;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.Message;
using Shared.ViewModel;

using Module.Settings.Factory.Support.Presentation;
using Module.Settings.Factory.Support.Pattern.Models;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.ViewModels
{
  [Export ("ModuleSettingsFactorySupportViewModel", typeof (IFactorySupportViewModel))]
  public class TFactorySupportViewModel : TViewModelAware<TFactorySupportModel>, IHandleMessageModule, IHandleMessageInternal, IFactorySupportViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactorySupportViewModel (IFactorySupportPresentation presentation)
      : base (new TFactorySupportModel ())
    {
      presentation.ViewModel = this;
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        if (message.IsAction (TMessageAction.Response)) {
          TDispatcher.BeginInvoke (NotifyDispatcher, message.Support.Argument.Types.ConnectionData);
        }
      }
    }

    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from child only
        if (message.Node.IsRelationChild) {
          if (message.IsAction (TInternalMessageAction.Change)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.Changed, TypeInfo);
            messageModule.Support.Argument.Types.ConnectionData.CopyFrom (message.Support.Argument.Types.ConnectionData);

            DelegateCommand.PublishMessage.Execute (messageModule);
          }
        }
      }
    }
    #endregion

    public void OnAuthenticationChecked (object tag)
    {
      if (tag is string authentication) {
        TAuthentication auth = (TAuthentication) Enum.Parse (typeof (TAuthentication), authentication);

        // to module
        var messageModule = new TFactoryMessage (TMessageAction.Authentication, TypeInfo);
        messageModule.Support.Argument.Types.Select (auth);

        DelegateCommand.PublishMessage.Execute (messageModule);
      }
    }

    #region Dispatcher
    void NotifyDispatcher (TDatabaseAuthentication SupportAuthentication)
    {
      // to sibiling
      var message = new TFactoryMessageInternal (TInternalMessageAction.DatabaseValidated, TypeInfo);
      message.Node.SelectRelationParent (TChild.Front);
      message.Support.Argument.Types.ConnectionData.CopyFrom (SupportAuthentication);

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