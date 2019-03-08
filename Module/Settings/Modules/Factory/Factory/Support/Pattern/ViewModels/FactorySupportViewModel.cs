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
  public class TFactorySupportViewModel : TViewModelAware<TFactorySupportModel>, IHandleMessageModule, IFactorySupportViewModel
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
        if (message.IsAction (TMessageAction.DatabaseValidated)) {
          if (message.Support.Argument.Types.EntityAction.Param1 is TComponentModelItem item) {
            Model.Select (item);

            RaiseChanged ();
          } 
        }
      }
    }
    #endregion

    #region Event
    public void OnApplyCommadClicked ()
    {
      if (Model.Validate ()) {
        TDispatcher.Invoke (ApplyDispatcher);
      }

      RaiseChanged ();
    }
    #endregion

    #region Dispatcher
    void ApplyDispatcher ()
    {
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Settings, Server.Models.Infrastructure.TOperation.Change, Server.Models.Infrastructure.TExtension.Settings);

      Model.Request (action);

      // to module
      var messageModule = new TFactoryMessage (TMessageAction.Request, TypeInfo);
      messageModule.Support.Argument.Types.Select (action);

      DelegateCommand.PublishMessage.Execute (messageModule);
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