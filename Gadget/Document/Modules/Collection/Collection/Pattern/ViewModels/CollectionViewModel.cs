/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Types;
using Shared.Resources;
using Shared.Message;
using Shared.ViewModel;

using Gadget.Collection.Presentation;
using Gadget.Collection.Pattern.Models;
//---------------------------//

namespace Gadget.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionViewModel", typeof (ICollectionViewModel))]
  public class TCollectionViewModel : TViewModelAware<TCollectionModel>, IHandleMessageModule, IHandleMessageInternal, ICollectionViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionViewModel (ICollectionPresentation presentation)
      : base (new TCollectionModel ())
    {
      TypeName = GetType ().Name;

      presentation.ViewModel = this;
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        // DatabaseValidated
        if (message.IsAction (TMessageAction.DatabaseValidated)) {
          // to child list
          var messageInternal = new TCollectionMessageInternal (TInternalMessageAction.DatabaseValidated, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.List);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }

        // RefreshProcess
        if (message.IsAction (TMessageAction.RefreshProcess)) {
          // to child list (Reload)
          var messageInternal = new TCollectionMessageInternal (TInternalMessageAction.Reload, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.List);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }

      // services
      if (message.IsModule (TResource.TModule.Services)) {
        // Response
        if (message.IsAction (TMessageAction.Response)) {
          // to child
          var messageInternal = new TCollectionMessageInternal (message.Result, TInternalMessageAction.Response, TypeInfo);
          messageInternal.Node.SelectRelationParent (message.Node.Child);
          messageInternal.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }

      // factory
      if (message.IsModule (TResource.TModule.Factory)) {
        // Reload
        if (message.IsAction (TMessageAction.Reload)) {
          // to child list
          var messageInternal = new TCollectionMessageInternal (TInternalMessageAction.Reload, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.List);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);

          // to module (Update)
          var messageModule = new TCollectionMessage (TMessageAction.Update, TypeInfo);
          messageModule.Node.SelectRelationModule (TChild.None);

          DelegateCommand.PublishMessage.Execute (messageModule);
        }
      }
    }

    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Collection)) {
        // from child only
        if (message.Node.IsRelationChild) {
          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            // to module
            var messageModule = new TCollectionMessage (TMessageAction.Request, TypeInfo);
            messageModule.Node.SelectRelationModule (message.Node.Child);
            messageModule.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

            DelegateCommand.PublishMessage.Execute (messageModule);
          }

          // Edit
          if (message.IsAction (TInternalMessageAction.Edit)) {
            // to module
            var messageModule = new TCollectionMessage (TMessageAction.Edit, TypeInfo);
            messageModule.Node.SelectRelationModule (TChild.None);
            messageModule.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

            DelegateCommand.PublishMessage.Execute (messageModule);
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