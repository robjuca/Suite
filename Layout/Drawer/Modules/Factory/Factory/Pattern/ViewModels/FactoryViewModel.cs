/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;

using Shared.Types;
using Shared.Message;
using Shared.Resources;
using Shared.ViewModel;

using Module.Factory.Presentation;
using Module.Factory.Pattern.Models;
//---------------------------//

namespace Module.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryViewModel", typeof (IFactoryViewModel))]
  public class TFactoryViewModel : TViewModelAware<TFactoryModel>, IHandleMessageModule, IHandleMessageInternal, IFactoryViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryViewModel (IFactoryPresentation presentation)
      : base (new TFactoryModel ())
    {
      TypeName = GetType ().Name;

      presentation.ViewModel = this;
      presentation.EventSubscribe (this);
    }
    #endregion

    #region Interface
    public void FocusEnter ()
    {
      // to module
      var messageModule = new TFactoryMessage (TMessageAction.EditEnter, TypeInfo);
      DelegateCommand.PublishMessage.Execute (messageModule);
    } 
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        // DatabaseValidated
        if (message.IsAction (TMessageAction.DatabaseValidated)) {
          // to child component list
          var messageInternal = new TFactoryMessageInternal (TInternalMessageAction.DatabaseValidated, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.List);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }

      // services
      if (message.IsModule (TResource.TModule.Services)) {
        // Response
        if (message.IsAction (TMessageAction.Response)) {
          // to child
          var messageInternal = new TFactoryMessageInternal (TInternalMessageAction.Response, TypeInfo);
          messageInternal.CopyResult (message.Result);
          messageInternal.Node.SelectRelationParent (message.Node.Child);
          messageInternal.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }

      // collection
      if (message.IsModule (TResource.TModule.Collection)) {
        // Edit
        if (message.IsAction (TMessageAction.Edit)) {
          // to child
          var messageInternal = new TFactoryMessageInternal (TInternalMessageAction.Edit, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.None);
          messageInternal.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }
    }

    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // only from child
        if (message.Node.IsRelationChild) {
          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            var requestData = message.Support.Argument.Types.RequestData;

            if (requestData.IsWhat (TWhat.None).IsFalse ()) {
              // to module
              var messageModule = new TFactoryMessage (TMessageAction.Request, TypeInfo);
              messageModule.Node.SelectRelationModule (message.Node.Child);
              messageModule.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

              DelegateCommand.PublishMessage.Execute (messageModule);
            }
          }

          // reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.Reload, TypeInfo);
            DelegateCommand.PublishMessage.Execute (messageModule);
          }

          // NavigateForm
          if (message.IsAction (TInternalMessageAction.NavigateForm)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.Focus, TypeInfo);
            messageModule.Support.Argument.Args.Select (message.Support.Argument.Args.Where);

            DelegateCommand.PublishMessage.Execute (messageModule);
          }

          // EditEnter
          if (message.IsAction (TInternalMessageAction.EditEnter)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.EditEnter, TypeInfo);
            DelegateCommand.PublishMessage.Execute (messageModule);
          }

          // EditLeave
          if (message.IsAction (TInternalMessageAction.EditLeave)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.EditLeave, TypeInfo);
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