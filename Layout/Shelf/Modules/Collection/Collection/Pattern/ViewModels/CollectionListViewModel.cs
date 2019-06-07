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

using Layout.Collection.Presentation;
using Layout.Collection.Pattern.Models;
//---------------------------//

namespace Layout.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionListViewModel", typeof (ICollectionListViewModel))]
  public class TCollectionListViewModel : TViewModelAware<TCollectionListModel>, IHandleMessageInternal, ICollectionListViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionListViewModel (ICollectionPresentation presentation)
      : base (new TCollectionListModel ())
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
        if (message.Node.IsParentToMe (TChild.List)) {
          // DatabaseValidated
          if (message.IsAction (TInternalMessageAction.DatabaseValidated)) {
            TDispatcher.Invoke (RequestDataDispatcher);
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Collection Full
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
              if (message.Result.IsValid) {
                // Shelf
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Shelf)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                }
              }
            }

            // Select Many
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many)) {
              if (message.Result.IsValid) {
                // Content
                //if (message.Support.Argument.Types.IsContextType (Server.Models.Infrastructure.TContextType.Context.Content)) {
                //  var entityAction = Server.Models.Content.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                //  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                //}
              }
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            TDispatcher.Invoke (ReloadDispatcher);
          }
        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            TDispatcher.Invoke (ReloadDispatcher);
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnSelectionChanged (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        TDispatcher.BeginInvoke (ItemSelectedDispatcher, item);
      }
    }
    #endregion

    #region Dispatcher
    void RequestDataDispatcher ()
    {
      // to parent
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full);

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RequestDataContentDispatcher ()
    {
      // to parent

      //var action = Server.Models.Content.TEntityAction.Create (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);
      //action.Select (Server.Models.Content.TContentOperation.TInternalOperation.Type);
      //action.SelectType (Server.Models.Infrastructure.TContextType.Context.Shelf);

      //var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      //message.Support.Argument.Types.Select (action);

      //DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Select (action);
      RaiseChanged ();

      RefreshCollection ("ModelItemsViewSource");
      //TDispatcher.Invoke (RequestDataContentDispatcher);
    }

    //void ResponseDataDispatcher (Server.Models.Content.TEntityAction action)
    //{
    //  Model.Select (action);
    //  RaiseChanged ();

    //  RefreshCollection ("ModelItemsViewSource");
    //}

    void ItemSelectedDispatcher (TComponentModelItem item)
    {
      // to Sibling display
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (item);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ReloadDispatcher ()
    {
      Model.Cleanup ();
      RaiseChanged ();

      // to Sibling display
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);

      TDispatcher.Invoke (RequestDataDispatcher);
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