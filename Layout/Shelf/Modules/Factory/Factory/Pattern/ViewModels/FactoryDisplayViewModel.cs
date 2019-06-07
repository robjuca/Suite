/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Types;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Shared.Layout.Shelf;

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

                // bag
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Bag)) {
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }

                else {
                  TDispatcher.BeginInvoke (ResponseSelectByIdDispatcher, action);
                }
              }
            }

            // Select - Many
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many)) {
              if (message.Result.IsValid) {
                // Image
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Image)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseSelectManyDispatcher, action);
                }
              }
            }
          }
        }

        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.Display)) {
          // Size
          if (message.IsAction (TInternalMessageAction.Size)) {
            if (message.Support.Argument.Args.Param1 is TSize size) {
              TDispatcher.BeginInvoke (SizeChangedDispatcher, size);
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

            TDispatcher.BeginInvoke (ContentMovedDispatcher, new Tuple<TPosition, TPosition>(position1, position2));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            TDispatcher.Invoke (CleanupDispatcher);
          }

          // Edit
          if (message.IsAction (TInternalMessageAction.Edit)) {
            var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
            TDispatcher.BeginInvoke (EditDispatcher, action);
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnComponentControlLoaded (object control)
    {
      if (control is TComponentDisplayControl) {
        m_ComponentControl = m_ComponentControl ?? (TComponentDisplayControl) control;
      }
    }
    #endregion

    #region Dispatcher
    void CleanupDispatcher ()
    {
      m_ComponentControl.Cleanup ();
      RaiseChanged ();
    }

    void ContentSelectedDispatcher (TContentInfo contentInfo)
    {
      contentInfo.Select (Server.Models.Infrastructure.TCategory.Bag);

      // to parent (Bag - Select - ById)
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Bag, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
      action.Id = contentInfo.Id;
      action.Param2 = contentInfo; // preserve

      TDispatcher.BeginInvoke (RequestDataDispatcher, action);
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

    void SizeChangedDispatcher (TSize size)
    {
      if ((size.Columns > 0) && (size.Rows > 0)) {
        Model.ChangeSize (size);
        m_ComponentControl.ChangeSize (size);
      }

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
      if (action.Param2 is TContentInfo contentInfo) {
        contentInfo.Select (TContentStyle.Mode.Horizontal, action.ModelAction.ExtensionLayoutModel.StyleHorizontal);
        contentInfo.Select (TContentStyle.Mode.Vertical, action.ModelAction.ExtensionLayoutModel.StyleVertical);

        Model.Select (contentInfo);

        // request node model 
        if (action.CollectionAction.ExtensionNodeCollection.Count > 0) {
          var node = action.CollectionAction.ExtensionNodeCollection [0];
          var childCategory = Server.Models.Infrastructure.TCategoryType.FromValue (node.ChildCategory);

          switch (childCategory) {
            case Server.Models.Infrastructure.TCategory.Document: {
                // (Select - ById)
                var entityAction = Server.Models.Component.TEntityAction.Create (childCategory, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById);
                entityAction.Id = node.ChildId;

                // to parent
                var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
                message.Support.Argument.Types.Select (entityAction);

                DelegateCommand.PublishInternalMessage.Execute (message);
              }
              break;

            case Server.Models.Infrastructure.TCategory.Image: {
                // (Select - Many) (Image)
                var entityAction = Server.Models.Component.TEntityAction.Create (childCategory, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);

                foreach (var nodeItem in action.CollectionAction.ExtensionNodeCollection) {
                  entityAction.IdCollection.Add (nodeItem.ChildId);
                }

                // to parent
                var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
                message.Support.Argument.Types.Select (entityAction);

                DelegateCommand.PublishInternalMessage.Execute (message);
              }
              break;

            case Server.Models.Infrastructure.TCategory.Video:
              break;
          }
        }
      }
    }

    void ResponseSelectByIdDispatcher (Server.Models.Component.TEntityAction action)
    {
      /*
       - action.ModelAction {child model (Document}
       - action.ModelAction.ExtensionNodeModel;
      */

      var node = action.ModelAction.ExtensionNodeModel;

      var model = Server.Models.Component.TComponentModel.Create (action.ModelAction);
      var childModelItem = TComponentModelItem.Create (model);
      childModelItem.Select (action.CategoryType.Category);

      var childId = Model.ContentInfo.Id;
      var childStyleHorizontal = Model.ContentInfo.StyleHorizontal;
      var childStyleVertical = Model.ContentInfo.StyleVertical;
      var position = Model.ContentInfo.Position;
      var childCategory = childModelItem.Category;

      var controlModel = Shared.Layout.Bag.TComponentControlModel.CreateDefault;
      controlModel.SelectModel (node.ParentId, Server.Models.Infrastructure.TCategoryType.FromValue (node.ParentCategory));
      controlModel.SelectChildModel (childId, childCategory, childStyleHorizontal, childStyleVertical, childModelItem);

      m_ComponentControl.InsertContent (position, controlModel);

      RaiseChanged ();
    }

    void ResponseSelectManyDispatcher (Server.Models.Component.TEntityAction action)
    {
      // - action.CollectionAction.EntityCollection {id, entityAction} (child Model - Image)

      var id = Model.ContentInfo.Id;
      var position = Model.ContentInfo.Position;
      var category = Model.ContentInfo.Category;

      var controlModel = Shared.Layout.Bag.TComponentControlModel.CreateDefault;
      controlModel.SelectModel (id, category);
      
      foreach (var item in action.CollectionAction.EntityCollection) {
        var modelAction = item.Value.ModelAction;

        var node = modelAction.ExtensionNodeModel;
        var childCategory = Server.Models.Infrastructure.TCategoryType.FromValue (node.ChildCategory);
        var childId = node.ChildId;

        var childStyleHorizontal = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
        childStyleHorizontal.Select (modelAction.ExtensionLayoutModel.StyleHorizontal);

        var childStyleVertical = TStyleInfo.Create (TContentStyle.Mode.Vertical);
        childStyleHorizontal.Select (modelAction.ExtensionLayoutModel.StyleVertical);

        var model = Server.Models.Component.TComponentModel.Create (modelAction);
        var childModelItem = TComponentModelItem.Create (model);
        childModelItem.Select (childCategory);
        childModelItem.GeometryModel.PositionIndex = int.Parse (node.Position);
        
        controlModel.SelectChildModel (childId, childCategory, childStyleHorizontal, childStyleVertical, childModelItem);
      }

      m_ComponentControl.InsertContent (position, controlModel);

      RaiseChanged ();
    }

    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      var controlModelCollection = new Dictionary<TPosition, Shared.Layout.Bag.TComponentControlModel> (); // bag child model (document or image or video)

      if (action.Param2 is Dictionary<TPosition, Shared.Layout.Bag.TComponentControlModel> childModels) {
        controlModelCollection = new Dictionary<TPosition, Shared.Layout.Bag.TComponentControlModel> (childModels); // child model
      }

      foreach (var item in controlModelCollection) {
        var position = item.Key;
        var controlModel = item.Value;

        var contentInfo = TContentInfo.CreateDefault;
        contentInfo.Select (controlModel.Id, position);
        contentInfo.Select (controlModel.Category);
        contentInfo.Select (TContentStyle.Mode.Horizontal, controlModel.HorizontalStyle.Style.ToString ());
        contentInfo.Select (TContentStyle.Mode.Vertical, controlModel.VerticalStyle.Style.ToString ());

        TDispatcher.BeginInvoke (ContentSelectedDispatcher, contentInfo);
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

    #region Fields
    TComponentDisplayControl                          m_ComponentControl; 
    #endregion
  };
  //---------------------------//

}  // namespace