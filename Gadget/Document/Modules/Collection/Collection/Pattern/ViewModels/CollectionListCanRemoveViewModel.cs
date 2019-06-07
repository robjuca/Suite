/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Gadget.Collection.Presentation;
using Gadget.Collection.Pattern.Models;
//---------------------------//

namespace Gadget.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionListCanRemoveViewModel", typeof (ICollectionListCanRemoveViewModel))]
  public class TCollectionListCanRemoveViewModel : TViewModelAware<TCollectionListCanRemoveModel>, IHandleMessageInternal, ICollectionListCanRemoveViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionListCanRemoveViewModel (ICollectionPresentation presentation)
      : base (new TCollectionListCanRemoveModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Collection)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.Filter)) {
          if (message.IsAction (TInternalMessageAction.Response)) {
            if (message.Result.IsValid) {
              // Document
              if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Document)) {
                if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Idle)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                }

                if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataSelectManyDispatcher, entityAction);
                }
              }

              // content
              //if (message.Support.Argument.Types.IsContextType (Server.Models.Infrastructure.TCategory.Content)) {
              //  if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many)) {
              //    var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
              //    TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
              //  }
              //}
            }
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.Filter)) {
          if (message.IsAction (TInternalMessageAction.Filter)) {
            TDispatcher.Invoke (FilterDispatcher);
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnBackCommadClicked ()
    {
      TDispatcher.Invoke (BackDispatcher);
    }

    public void OnApplyOkCommadClicked ()
    {
      
    }

    public void OnStyleSelected (string style)
    {
      Enum.TryParse (style, out TContentStyle.Style selectedStyle);

      Model.SelectStyle (selectedStyle);
      RaiseChanged ();
    }

    public void OnCheckAllChecked ()
    {
      Model.SelectAll ();
      RaiseChanged ();
    }

    public void OnCheckAllUnchecked ()
    {
      Model.UnselectAll ();
      RaiseChanged ();
    }
    #endregion

    #region Dispatcher
    void BackDispatcher ()
    {
      // to Sibling
      var message = new TCollectionMessageInternal (TInternalMessageAction.Back, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Filter);

      DelegateCommand.PublishInternalMessage.Execute (message);

      TDispatcher.Invoke (EditLeaveDispatcher);
    }

    void EditEnterDispatcher ()
    {
      // to parent
      var msg = new TFactoryMessageInternal (TInternalMessageAction.EditEnter, TypeInfo);
      msg.Node.SelectRelationChild (TChild.Filter);

      DelegateCommand.PublishInternalMessage.Execute (msg);

      // to Sibling
      var message = new TCollectionMessageInternal (TInternalMessageAction.FilterEnter, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Filter);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void EditLeaveDispatcher ()
    {
      // to parent
      var msg = new TFactoryMessageInternal (TInternalMessageAction.EditLeave, TypeInfo);
      msg.Node.SelectRelationChild (TChild.Filter);

      DelegateCommand.PublishInternalMessage.Execute (msg);

      // to Sibling
      var message = new TCollectionMessageInternal (TInternalMessageAction.FilterLeave, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Filter);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void FilterDispatcher ()
    {
      // request Document idle collection
      // to parent
      //var msg = new TFactoryMessageInternal (TInternalMessageAction.Request, TypeInfo);
      //msg.Node.SelectRelationChild (TChild.Filter);
      //msg.Support.Argument.Types.Select (Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Idle));

      //DelegateCommand.PublishInternalMessage.Execute (msg);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction entityAction)
    {
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Document, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);

      foreach (var item in entityAction.CollectionAction.ModelCollection) {
        //action.IdCollection.Add (item.Value.DocumentInfo.DocumentId);
      }

      // request content select many 
      // to parent
      var msg = new TFactoryMessageInternal (TInternalMessageAction.Request, TypeInfo);
      msg.Node.SelectRelationChild (TChild.Filter);
      msg.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (msg);
    }

    //void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    //{
    //  var idList = new Collection<Guid> ();

    //  if (Model.Select (action, idList)) {
    //    var DocumentAction = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);
    //    DocumentAction.SelectMany (idList);

    //    // request Document select many 
    //    // to parent
    //    var msg = new TFactoryMessageInternal (TInternalMessageAction.Request, TypeInfo);
    //    msg.Node.SelectRelationChild (TChild.Filter);
    //    msg.Support.Argument.Types.Select (DocumentAction);

    //    DelegateCommand.PublishInternalMessage.Execute (msg);
    //  }
    //}

    void ResponseDataSelectManyDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Select (action);
      RaiseChanged ();

      TDispatcher.Invoke (EditEnterDispatcher);
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