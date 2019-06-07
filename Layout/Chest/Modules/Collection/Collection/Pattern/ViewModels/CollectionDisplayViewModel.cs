/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;

using Shared.Layout.Chest;

using Layout.Collection.Presentation;
using Layout.Collection.Pattern.Models;
//---------------------------//

namespace Layout.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionDisplayViewModel", typeof (ICollectionDisplayViewModel))]
  public class TCollectionDisplayViewModel : TViewModelAware<TCollectionDisplayModel>, IHandleMessageInternal, ICollectionDisplayViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionDisplayViewModel (ICollectionPresentation presentation)
      : base (new TCollectionDisplayModel ())
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
        // From Parent
        if (message.Node.IsParentToMe (TChild.Display)) {
          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Select - ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, entityAction);
              }
            }

            // Remove
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Remove)) {
              if (message.Result.IsValid) {
                TDispatcher.Invoke (CleanupDispatcher);
                TDispatcher.Invoke (ReloadDispatcher);
              }
            }

            // Change - Active
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Change, Server.Models.Infrastructure.TExtension.Active)) {
              if (message.Result.IsValid) {
                TDispatcher.Invoke (RefreshDispatcher);
              }
            }
          }
        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.Display)) {
          // select
          if (message.IsAction (TInternalMessageAction.Select)) {
            TDispatcher.BeginInvoke (ItemSelectedDispatcher, message.Support.Argument.Types.Item);
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            TDispatcher.Invoke (CleanupDispatcher);
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnComponentControlLoaded (object control)
    {
      if (control is TComponentDisplayControl displayControl) {
        m_ComponentControl = m_ComponentControl ?? displayControl;
      }
    }
    #endregion

    #region Event
    public void OnEditCommadClicked ()
    {
      TDispatcher.Invoke (EditDispatcher);
    }

    public void OnRemoveCommadClicked ()
    {
      TDispatcher.Invoke (RemoveDispatcher);
    }

    public void OnActiveClicked ()
    {
      TDispatcher.Invoke (ActiveDispatcher);
    }
    #endregion

    #region Dispatcher
    void CleanupDispatcher ()
    {
      Cleanup ();
    }

    void ReloadDispatcher ()
    {
      // to Sibling
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Reload, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RefreshDispatcher ()
    {
      // to Sibling
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Refresh, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ItemSelectedDispatcher (TComponentModelItem item)
    {
      // item: component
      Cleanup ();

      Model.Select (item);
      RaiseChanged ();

      // to parent
      // Select - ById
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Chest, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
      action.Id = item.Id;

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseSelectByIdDispatcher (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();
      
      Model.SelectById (action);

      m_ComponentControl.InsertContent (Model.RequestChildCollection ()); // insert component

      RaiseChanged ();
    }

    void EditDispatcher ()
    {
      var entityAction = Server.Models.Component.TEntityAction.CreateDefault;
      Model.RequestModel (entityAction);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Edit, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RemoveDispatcher ()
    {
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Remove);
      Model.RequestModel (action);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ActiveDispatcher ()
    {
      var entityAction = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Chest, Server.Models.Infrastructure.TOperation.Change, Server.Models.Infrastructure.TExtension.Active);
      Model.RequestStatus (entityAction);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (entityAction);

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

    #region Fields
    TComponentDisplayControl                                    m_ComponentControl;
    #endregion

    #region Support
    void Cleanup ()
    {
      Model.Cleanup ();

      RaiseChanged ();
    }
    #endregion
  };
  //---------------------------//

}  // namespace