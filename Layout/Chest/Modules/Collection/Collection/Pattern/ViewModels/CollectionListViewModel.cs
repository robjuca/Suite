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
                // Chest
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Chest)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                }
              }
            }

            // Select - ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                // Chest
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Chest)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, entityAction);
                }
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

          if (message.IsAction (TInternalMessageAction.Refresh)) {
            TDispatcher.Invoke (RefreshDispatcher);
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
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Chest, Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full);

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Select (action);
      RaiseChanged ();

      RefreshCollection ("ModelItemsViewSource");
    }

    void ResponseSelectByIdDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.ActiveChanged (action);

      RaiseChanged ();

      RefreshCollection ("ModelItemsViewSource");

      ItemSelectedDispatcher (Model.Current);
    }

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

    void RefreshDispatcher ()
    {
      var id = Model.CurrentId;

      if (id.NotEmpty ()) {
        // to parent
        var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Chest, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
        action.Id = id;

        var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
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