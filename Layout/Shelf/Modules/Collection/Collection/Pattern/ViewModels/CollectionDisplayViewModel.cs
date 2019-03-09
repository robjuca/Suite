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

using Shared.Module.Shelf;

using Module.Collection.Presentation;
using Module.Collection.Pattern.Models;
//---------------------------//

namespace Module.Collection.Pattern.ViewModels
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
            // Select - Relation
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation)) {
              if (message.Result.IsValid) {
                var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                TDispatcher.BeginInvoke (ResponseRelationDispatcher, entityAction);
              }
            }

            // Select - Many
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many)) {
              if (message.Result.IsValid) {
                // operation Bag only
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Bag)) {
                  var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseComponentDispatcher, entityAction);
                }
              }
            }

            // Remove
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Remove)) {
              if (message.Result.IsValid) {
                TDispatcher.Invoke (CleanupDispatcher);
                TDispatcher.Invoke (ReloadDispatcher);
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.Display)) {
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
        m_DisplayControl = m_DisplayControl ?? displayControl;
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
    #endregion

    #region Dispatcher
    void CleanupDispatcher ()
    {
      Cleanup ();
    }

    void ReloadDispatcher ()
    {
      // to sibiling
      var message = new TCollectionSibilingMessageInternal (TInternalMessageAction.Reload, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ItemSelectedDispatcher (TComponentModelItem item)
    {
      // item: Shelf component
      Cleanup ();

      m_DisplayControl.ChangeSize (item.Size);

      Model.Select (item);
      RaiseChanged ();

      // to parent
      // request component relation (using ComponentOperation support)
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation);
      action.CollectionAction.SelectComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Id);
      action.ComponentOperation.SelectById (item.Id);

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RequestComponentDispatcher ()
    {
      // to parent
      // request bags
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Bag, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);
      Model.RequestComponentId (action);

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseRelationDispatcher (Server.Models.Component.TEntityAction action)
    {
      /*
       DATA:
        action.ComponentOperation
      */

      action.ThrowNull ();

      // add bags (etapa 1de2)
      Model.SelectRelation (action);

      TDispatcher.Invoke (RequestComponentDispatcher);
    }

    void ResponseComponentDispatcher (Server.Models.Component.TEntityAction action)
    {
      /*
       DATA:
        action.CollectionAction.ModelActionCollection {id, model}
      */

      action.ThrowNull ();

      // update bags (etapa 2de2)
      Model.SelectComponent (action);

      // insert bag
      foreach (var model in Model.ControlModelCollection) {
        var position = model.Key;
        var controlModel = model.Value;

        m_DisplayControl.InsertContent (position, controlModel);
      }

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
    TComponentDisplayControl                          m_DisplayControl;
    #endregion

    #region Support
    void Cleanup ()
    {
      Model.Cleanup ();
      m_DisplayControl.Cleanup ();

      RaiseChanged ();
    } 
    #endregion
  };
  //---------------------------//

}  // namespace