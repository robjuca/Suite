/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using GongSolutions.Wpf.DragDrop;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;
using Shared.DashBoard;

using Layout.Factory.Presentation;
using Layout.Factory.Pattern.Models;
//---------------------------//

namespace Layout.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryComponentListViewModel", typeof (IFactoryComponentListViewModel))]
  public class TFactoryComponentListViewModel : TViewModelAware<TFactoryComponentListModel>, IHandleMessageInternal, IFactoryComponentListViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryComponentListViewModel (IFactoryPresentation presentation)
      : base (new TFactoryComponentListModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);

      DragDropHandler = new TDragDropHandler (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // parent to me
        if (message.Node.IsParentToMe (TChild.List)) {
          // DatabaseValidated
          if (message.IsAction (TInternalMessageAction.DatabaseValidated)) {
            TDispatcher.Invoke (RequestComponentRelationDispatcher);
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Select - Relation
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation)) {
              if (message.Result.IsValid) {
                // shelf
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Shelf)) {
                  TDispatcher.BeginInvoke (ResponseComponentRelationDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
                }
              }
            }

            // Select - Zap
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Zap)) {
              if (message.Result.IsValid) {
                // bag
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Bag)) {
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
                }
              }
            }

            // Select - ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                TDispatcher.BeginInvoke (ResponseComponentDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.List)) {
          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            TDispatcher.Invoke (CleanupDispatcher);
          }

          // Drop
          if (message.IsAction (TInternalMessageAction.Drop)) {
            TDispatcher.BeginInvoke (DropDispatcher, message.Support.Argument.Args.Id);
          }

          // Refresh
          if (message.IsAction (TInternalMessageAction.Refresh)) {
            TDispatcher.Invoke (RequestComponentRelationDispatcher);
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnStyleHorizontalSelected (string style)
    {
      Enum.TryParse (style, out TContentStyle.Style selectedStyle);

      Model.SelectStyleHorizontal (selectedStyle);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    public void OnStyleVerticalSelected (string style)
    {
      Enum.TryParse (style, out TContentStyle.Style selectedStyle);

      Model.SelectStyleVertical (selectedStyle);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    public void OnStyleSelected (string style)
    {
      Enum.TryParse (style, out TContentStyle.Style selectedStyle);

      //TODO: ????
      //Model.SelectStyle (selectedStyle);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("ComponentCollectionViewSource");
    }

    void CleanupDispatcher ()
    {
      Model.SelectDefault ();

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void RequestComponentRelationDispatcher ()
    {
      // to parent (Select - Relation)
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation);
      action.CollectionAction.SelectComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Category);
      action.ComponentOperation.SelectByCategory (Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Shelf));

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RequestDataDispatcher ()
    {
      // to parent (Select - Zap)
      //bag
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Bag, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Zap);
      Model.RequestRelations (action);

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseComponentRelationDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.SelectComponentRelation (action);

      TDispatcher.Invoke (RequestDataDispatcher);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void ResponseComponentDispatcher (Server.Models.Component.TEntityAction action)
    {
      var id = Model.TryToInsert (action);

      TDispatcher.BeginInvoke (DropRestoreDispatcher, id);
    }

    void DropDispatcher (Guid id)
    {
      Model.DropComponentModel (id);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void RestoreDispatcher (TComponentModelItem modelItem)
    {
      if (Model.RestoreComponentModel (modelItem.Id)) {
        TDispatcher.BeginInvoke (DropRestoreDispatcher, modelItem.Id);
      }

      else {
        // request component by Id (Select - ById)
        var action = Server.Models.Component.TEntityAction.Create (modelItem.Category, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
        action.Id = modelItem.Id;

        // to Parent
        var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void DropRestoreDispatcher (Guid id)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Drop, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (id);

      DelegateCommand.PublishInternalMessage.Execute (message);

      TDispatcher.Invoke (RefreshAllDispatcher);
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

    #region Drop Handler
    public TDragDropHandler DragDropHandler
    {
      get;
      private set;
    }
    #endregion

    #region DragDrop
    //-----TDragDropHandler
    public class TDragDropHandler : IDropTarget
    {
      #region IDragDrop
      void IDropTarget.DragOver (IDropInfo dropInfo)
      {
        if (ValidateDrop (dropInfo)) {
          dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
          dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
      }

      void IDropTarget.Drop (IDropInfo dropInfo)
      {
        if (ValidateDrop (dropInfo)) {
          var externalData = dropInfo.Data as TDashBoardItem;
          var category = externalData.Category;

          var modelItem = TComponentModelItem.CreateDefault;
          modelItem.Select (externalData.Id, category);

          TDispatcher.BeginInvoke (m_Parent.RestoreDispatcher, modelItem);
        }
      }
      #endregion

      #region Constructor
      public TDragDropHandler (TFactoryComponentListViewModel parent)
      {
        m_Parent = parent;
      }
      #endregion

      #region Fields
      readonly TFactoryComponentListViewModel                             m_Parent;
      #endregion

      #region Support
      bool ValidateDrop (IDropInfo dropInfo)
      {
        if (dropInfo.Data is TDashBoardItem externalData) {
          if (externalData.IsBusy) {
            return (true);
          }
        }

        return (false);
      }
      #endregion
    };
    #endregion
  };
  //---------------------------//

}  // namespace