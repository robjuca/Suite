/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Types;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;

using Shared.Layout.Drawer;

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
                // Shelf
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Shelf)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                }
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.Display)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            if (message.Support.Argument.Args.PropertyName.Equals ("Int4Property")) {
              TDispatcher.BeginInvoke (PropertySelectDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
            }
          }

          // select
          if (message.IsAction (TInternalMessageAction.Select)) {
            if (message.Support.Argument.Args.Param1 is TContentInfo contentInfo) {
              TDispatcher.BeginInvoke (ContentSelectedDispatcher, contentInfo);
            }
          }

          // Remove
          if (message.IsAction (TInternalMessageAction.Remove)) {
            TDispatcher.BeginInvoke (ContentRemovedDispatcher, message.Support.Argument.Args.Id);
          }

          // Move
          if (message.IsAction (TInternalMessageAction.Move)) {
            var position1 = (TPosition) message.Support.Argument.Args.Param1;
            var position2 = (TPosition) message.Support.Argument.Args.Param2;

            TDispatcher.BeginInvoke (ContentMovedDispatcher, new Tuple<TPosition, TPosition> (position1, position2));
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
        m_ComponentControl = m_ComponentControl ?? displayControl;
      }
    }
    #endregion

    #region Dispatcher
    void CleanupDispatcher ()
    {
      m_ComponentControl.Cleanup ();

      RaiseChanged ();
    }

    void PropertySelectDispatcher (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      int cols = action.ModelAction.ExtensionGeometryModel.SizeCols;
      int rows = action.ModelAction.ExtensionGeometryModel.SizeRows;

      if ((cols > 0) && (rows > 0)) {
        m_ComponentControl.ChangeSize (TSize.Create (cols, rows));
      }

      RaiseChanged ();
    }

    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      //action.Id 
      //action.ModelAction {model}
      //action.Param1 = Collection<TComponentModelItem> {child collection}

      Model.SelectModel (action);

      m_ComponentControl.Cleanup ();
      m_ComponentControl.InsertContent (Model.ComponentModelItem.ChildCollection);

      RaiseChanged ();
    }

    void ContentSelectedDispatcher (TContentInfo contentInfo)
    {
      contentInfo.ThrowNull ();

      // Select - ById
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
      action.Id = contentInfo.Id;
      action.Param2 = contentInfo;  // preserve

      RequestDataDispatcher (action);
    }

    void ContentRemovedDispatcher (Guid id)
    {
      m_ComponentControl.RemoveContent (id);

      RaiseChanged ();
    }

    void ContentMovedDispatcher (Tuple<TPosition, TPosition> tuple)
    {
      m_ComponentControl.DoMove (tuple.Item1, tuple.Item2);

      RaiseChanged ();
    }

    void RequestDataDispatcher (Server.Models.Infrastructure.IEntityAction action)
    {
      // to parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      // action.ModelAction {shelf model}
      // action.CollectionAction.EntityCollection {id, model}
      // action.CollectionAction.ComponentOperation

      var position = TPosition.CreateDefault;

      if (action.Param2 is TContentInfo contentInfo) {
        position.CopyFrom (contentInfo.Position);
      }

      action.ModelAction.ExtensionGeometryModel.PositionCol = position.Column;
      action.ModelAction.ExtensionGeometryModel.PositionRow = position.Row;

      m_ComponentControl.InsertContent (action);
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