/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;

using Shared.Layout.Chest;

using Layout.Factory.Presentation;
using Layout.Factory.Pattern.Models;
//---------------------------//

namespace Layout.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryDisplayViewModel", typeof (IFactoryDisplayViewModel))]
  public class TFactoryDisplayViewModel : TViewModelAware<TFactoryDisplayModel>, IHandleMessageInternal, IFactoryDisplayViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryDisplayViewModel (IFactoryPresentation presentation)
      : base (new TFactoryDisplayModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.Display)) {
          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Select - ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, action);
              }
            }
          }
        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.Display)) {
          // Insert
          if (message.IsAction (TInternalMessageAction.Insert)) {
            if (message.Support.Argument.Args.Param1 is TComponentItemInfo item) {
              TDispatcher.BeginInvoke (InsertContentDispatcher, item);
            }
          }

          // Move
          if (message.IsAction (TInternalMessageAction.Move)) {
            if (message.Support.Argument.Args.Param1 is Dictionary <Guid, int> list) {
              TDispatcher.BeginInvoke (MoveContentDispatcher, list);
            }
          }

          // Remove
          if (message.IsAction (TInternalMessageAction.Remove)) {
            if (message.Support.Argument.Args.Param1 is TComponentItemInfo item) {
              TDispatcher.BeginInvoke (RemoveContentDispatcher, item);
            }
          }

          // Edit
          if (message.IsAction (TInternalMessageAction.Edit)) {
            var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
            TDispatcher.BeginInvoke (EditDispatcher, action);
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
      if (control is TComponentDesignControl displayControl) {
        bool cleanup = m_ComponentControl.IsNull ();
        m_ComponentControl = m_ComponentControl ?? displayControl;

        if (cleanup) {
          m_ComponentControl.Cleanup ();
        }
      }
    }
    #endregion

    #region Dispatcher
    void CleanupDispatcher ()
    {
      m_ComponentControl.Cleanup ();
      RaiseChanged ();
    }

    void InsertContentDispatcher (TComponentItemInfo item)
    {
      // to parent
      // Drawer Select - ById
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Drawer, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
      action.Id = item.Id;

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RemoveContentDispatcher (TComponentItemInfo item)
    {
      m_ComponentControl.RemoveContent (item.Id);

      RaiseChanged ();
    }

    void MoveContentDispatcher (Dictionary <Guid, int> list)
    {
      if (list.Count.Equals (1)) {
        foreach (var item in list) {
          var id = item.Key;
          var position = item.Value;

          m_ComponentControl.MoveContent (id, position);
        }
      }

      RaiseChanged ();
    }

    void ResponseSelectByIdDispatcher (Server.Models.Component.TEntityAction action)
    {
      // action.ModelAction { Drawer model }
      // action.ComponentOperation.ParentIdCollection [id {Drawer}];
      // action.CollectionAction.EntityCollection { id, action } Drawer relation (Shelf)

      // Drawer model
      var modelItem = TComponentModelItem.Create (action);
      modelItem.RequestChild (action);

      m_ComponentControl.InsertContent (modelItem);

      RaiseChanged ();
    }

    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      //action.Id // Chest Id
      //action.ModelAction // Chest model
      //action.Param1 = new Collection<TComponentModelItem>  // Chest child collection {Drawer}

      action.ThrowNull ();

      if (action.Param1 is Collection<TComponentModelItem> childCollection) {
        m_ComponentControl.InsertContent (childCollection);
      }

      RaiseChanged ();
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
    TComponentDesignControl                           m_ComponentControl;
    #endregion
  };
  //---------------------------//

}  // namespace