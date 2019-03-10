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

using Layout.Factory.Presentation;
using Layout.Factory.Pattern.Models;
//---------------------------//

namespace Layout.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryListViewModel", typeof (IFactoryListViewModel))]
  public class TFactoryListViewModel : TViewModelAware<TFactoryListModel>, IHandleMessageInternal, IFactoryListViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListViewModel (IFactoryPresentation presentation)
      : base (new TFactoryListModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);

      InputDragDropHandler = new TDragDropHandler (this, TDragDropHandler.TMode.Input);
      OutputDragDropHandler = new TDragDropHandler (this, TDragDropHandler.TMode.Output);
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
            // Select Relation
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation)) {
              if (message.Result.IsValid) {
                // chest
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Chest)) {
                  TDispatcher.BeginInvoke (ResponseComponentRelationDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
                }
              }
            }

            // Select Zap
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Zap)) {
              if (message.Result.IsValid) {
                // drawer
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Drawer)) {
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
                }
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.List)) {
          // Edit
          if (message.IsAction (TInternalMessageAction.Edit)) {
            TDispatcher.BeginInvoke (EditDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestModelDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            TDispatcher.Invoke (CleanupDispatcher);
          }
        }
      }
    }
    #endregion

    #region Dispatcher
    void RefreshAllCollectionDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("InputListViewSource");
      RefreshCollection ("OutputListViewSource");
    }

    void CleanupDispatcher ()
    {
      Model.Cleanup ();

      TDispatcher.Invoke (RequestComponentRelationDispatcher);
    }

    void RequestComponentRelationDispatcher ()
    {
      // to parent
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Chest, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation);
      action.CollectionAction.SelectComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Category);
      action.ComponentOperation.SelectByCategory (Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Chest));

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RequestDataDispatcher ()
    {
      // to parent
      //drawer Select - Zap
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Drawer, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Zap);
      Model.RequestRelations (action);

      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RequestModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.RequestModel (action);

      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Response, TChild.List, TypeInfo);
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

      TDispatcher.Invoke (RefreshAllCollectionDispatcher);
    }

    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.SelectModel (action);

      TDispatcher.Invoke (RefreshAllCollectionDispatcher);
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
    public TDragDropHandler InputDragDropHandler
    {
      get;
      private set;
    }

    public TDragDropHandler OutputDragDropHandler
    {
      get;
      private set;
    }
    #endregion

    #region DragDrop
    //-----TDragDropHandler
    public class TDragDropHandler : DefaultDropHandler
    {
      #region Data
      public enum TMode
      {
        Input,
        Output
      };
      #endregion

      #region Overrides
      public override void DragOver (IDropInfo dropInfo)
      {
        if (ValidateDrop (dropInfo)) {
          dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
          dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }
      }

      public override void Drop (IDropInfo dropInfo)
      {
        if (ValidateDrop (dropInfo)) {
          var dropData = dropInfo.Data as TComponentItemInfo;

          switch (Mode) {
            case TMode.Input:
              m_Parent.DropToInput (dropData);
              break;

            case TMode.Output:
              if (m_DropToMySelf) {
                base.Drop (dropInfo);

                m_Parent.DropMove (dropData);
              }

              else {
                m_Parent.DropToOutput (dropData);
              }
              break;
          }
        }
      }
      #endregion

      #region Constructor
      public TDragDropHandler (TFactoryListViewModel parent, TMode mode)
      {
        m_Parent = parent;
        Mode = mode;
      }
      #endregion

      #region Property
      TMode Mode
      {
        get;
      } 
      #endregion

      #region Fields
      TFactoryListViewModel                           m_Parent;
      bool                                            m_DropToMySelf;
      #endregion

      #region Support
      bool ValidateDrop (IDropInfo dropInfo)
      {
        m_DropToMySelf = false;

        if (dropInfo.Data is TComponentItemInfo externalData) {
          string sourceName = string.Empty;
          string targetName = string.Empty;

          if (dropInfo.DragInfo.VisualSource is System.Windows.Controls.ListView visualSource) {
            sourceName = visualSource.Name;
          }

          if (dropInfo.VisualTarget is System.Windows.Controls.ListView visualTarget) {
            targetName = visualTarget.Name;
          }

          if (string.IsNullOrEmpty (sourceName).IsFalse () && string.IsNullOrEmpty (targetName).IsFalse ()) {
            switch (Mode) {
              case TMode.Input:
                if (sourceName.Equals (targetName).IsFalse ()) {
                  if (sourceName.Equals ("OutputList")) {
                    return (true);
                  }
                }
                break;

              case TMode.Output:
                if (sourceName.Equals ("InputList")) {
                  return (true);
                }

                if (sourceName.Equals ("OutputList")) {
                  m_DropToMySelf = true;
                  return (true);
                }
                break;
            }
          }
        }

        return (false);
      }
      #endregion
    };
    #endregion

    #region Suppot
    void DropToInput (TComponentItemInfo item)
    {
      Model.DropToInput (item);

      TDispatcher.Invoke (RefreshAllCollectionDispatcher);

      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Remove, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (item, null);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void DropToOutput (TComponentItemInfo item)
    {
      Model.DropToOutput (item);

      TDispatcher.Invoke (RefreshAllCollectionDispatcher);

      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Insert, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (item, null);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void DropMove (TComponentItemInfo item)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Move, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (Model.RequestOutputPosition (item.Id), null); // use param1

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion
  };
  //---------------------------//

}  // namespace