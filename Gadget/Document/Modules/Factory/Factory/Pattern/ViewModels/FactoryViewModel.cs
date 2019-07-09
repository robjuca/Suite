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

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
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

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        // RefreshProcess
        if (message.IsAction (TMessageAction.RefreshProcess)) {
          // to child property (Edit Leave)
          var messageInternal = new TFactoryMessageInternal (TInternalMessageAction.EditLeave, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.Property);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }

      // services
      if (message.IsModule (TResource.TModule.Services)) {
        // Response
        if (message.IsAction (TMessageAction.Response)) {
          // to child
          var messageInternal = new TFactoryMessageInternal (TInternalMessageAction.Response, TypeInfo);
          messageInternal.Node.SelectRelationParent (message.Node.Child);
          messageInternal.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }

      //collection
      if (message.IsModule (TResource.TModule.Collection)) {
        // Edit
        if (message.IsAction (TMessageAction.Edit)) {
          // to child edit 
          var messageInternal = new TFactoryMessageInternal (TInternalMessageAction.Edit, TypeInfo);
          messageInternal.Node.SelectRelationParent (TChild.Property);
          messageInternal.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

          DelegateCommand.PublishInternalMessage.Execute (messageInternal);
        }
      }
    }

    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from child only
        if (message.Node.IsRelationChild) {
          // request
          if (message.IsAction (TInternalMessageAction.Request)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.Request, TypeInfo);
            messageModule.Node.SelectRelationModule (message.Node.Child);
            messageModule.Support.Argument.Types.CopyFrom (message.Support.Argument.Types);

            DelegateCommand.PublishMessage.Execute (messageModule);
          }

          // reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            // to module
            DelegateCommand.PublishMessage.Execute (new TFactoryMessage (TMessageAction.Reload, TypeInfo));
          }

          // navigateform
          if (message.IsAction (TInternalMessageAction.NavigateForm)) {
            // to module
            var messageModule = new TFactoryMessage (TMessageAction.Focus, TypeInfo);
            messageModule.Support.Argument.Args.Select (message.Support.Argument.Args.Where);

            DelegateCommand.PublishMessage.Execute (messageModule);
          }

          // edit enter
          if (message.IsAction (TInternalMessageAction.EditEnter)) {
            // to module
            DelegateCommand.PublishMessage.Execute (new TFactoryMessage (TMessageAction.EditEnter, TypeInfo));
          }

          // edit leave
          if (message.IsAction (TInternalMessageAction.EditLeave)) {
            // to module
            DelegateCommand.PublishMessage.Execute (new TFactoryMessage (TMessageAction.EditLeave, TypeInfo));
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