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
            // Collection - Minimum
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Minimum)) {
              if (message.Result.IsValid) {
                // Document
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Document)) {
                  var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, action);
                }
              }
            }

            // Select - ById
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.ById)) {
              if (message.Result.IsValid) {
                var action = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                TDispatcher.BeginInvoke (ResponseModelDispatcher, action);
              }
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            Model.PreserveCurrent ();
            Model.Cleanup ();

            TDispatcher.Invoke (RefreshAllDispatcher);
            TDispatcher.Invoke (RequestDataDispatcher);
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            Model.Cleanup ();

            TDispatcher.Invoke (RefreshAllDispatcher);
            TDispatcher.Invoke (RequestDataDispatcher);
          }

          // Select
          if (message.IsAction (TInternalMessageAction.Style)) {
            TDispatcher.BeginInvoke (StyleHorizontalChangedDispatcher, message.Support.Argument.Types.HorizontalStyle.StyleString);
            TDispatcher.BeginInvoke (StyleVerticalChangedDispatcher, message.Support.Argument.Types.VerticalStyle.StyleString);
          }

          // Back
          if (message.IsAction (TInternalMessageAction.Back)) {
            Model.SlideIndex = 0;

            TDispatcher.Invoke (RefreshAllDispatcher);
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

    public void OnSelectionChanged (TComponentModelItem item)
    {
      TDispatcher.BeginInvoke (ItemSelectedDispatcher, item);
    }

    public void OnFilterCanRemoveClicked ()
    {
      Model.SlideIndex = 1;
      TDispatcher.Invoke (RefreshAllDispatcher);

      //to Sibling
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Filter, TChild.List, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    public void OnDashBoardClicked ()
    {
      Model.SlideIndex = 2;
      TDispatcher.Invoke (RefreshAllDispatcher);

      //to Sibling
      var action = Server.Models.Component.TEntityAction.CreateDefault;
      action.Summary.Select (Server.Models.Infrastructure.TCategory.Document);

      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Summary, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    public void OnFilterEnabledChanged (string filter)
    {
      // TODO:filter
      //var message = new TDocumentModuleInternalMessage ();
      //message.SelectSender (new TTypeInfo (TypeName));
      //message.Action.Select (TMessageActionInternals.Cleanup);
      //DelegateCommand.PublishModuleInternalMessage.Execute (message);

      //Model.DocumentFilter.SelectFilterEnabled (filter);
      //THelper.DispatcherLater (PrepareToApplyFilterDispatcher);
    }

    public void OnFilterPictureChanged (string filter)
    {
      // TODO:filter
      //var message = new TDocumentModuleInternalMessage ();
      //message.SelectSender (new TTypeInfo (TypeName));
      //message.Action.Select (TMessageActionInternals.Cleanup);
      //DelegateCommand.PublishModuleInternalMessage.Execute (message);

      //Model.DocumentFilter.SelectFilterPicture (filter);
      //THelper.DispatcherLater (PrepareToApplyFilterDispatcher);
    }

    public void OnFilterSearchCommadClicked ()
    {
      // TODO:filter
      //if (Model.DocumentFilter.ValidateSearch ()) {
      //  var message = new TDocumentModuleInternalMessage ();
      //  message.SelectSender (new TTypeInfo (TypeName));
      //  message.Action.Select (TMessageActionInternals.Cleanup);
      //  DelegateCommand.PublishModuleInternalMessage.Execute (message);

      //  THelper.DispatcherLater (PrepareToApplyFilterDispatcher);
      //}
    }

    public void OnFilterCleanCommadClicked ()
    {
      // TODO:filter
      //Model.CleanSearch ();

      //THelper.DispatcherLater (PrepareToApplyFilterDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      RefreshCollection ("ModelItemsViewSource");
    }

    void RequestDataDispatcher ()
    {
      // to parent
      // Collection - Minimum
      var action = Server.Models.Component.TEntityAction.Create (
        Server.Models.Infrastructure.TCategory.Document,
        Server.Models.Infrastructure.TOperation.Collection,
        Server.Models.Infrastructure.TExtension.Minimum
      );

      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.PreserveCurrent ();
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
      TDispatcher.Invoke (TryToSelectDispatcher);
    }

    void RequestModelDispatcher ()
    {
      if (Model.Id.NotEmpty ()) {
        // Select - ById
        var action = Server.Models.Component.TEntityAction.Create (
          Server.Models.Infrastructure.TCategory.Document,
          Server.Models.Infrastructure.TOperation.Select,
          Server.Models.Infrastructure.TExtension.ById);

        action.Id = Model.Id;

        // to parent
        var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void ResponseModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Update (action);

      // to Sibling
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (Model.Current);

      DelegateCommand.PublishInternalMessage.Execute (message);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void ItemSelectedDispatcher (TComponentModelItem item)
    {
      if (item.IsNull ()) {
        // to Sibling
        var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }

      else {
        TDispatcher.Invoke (RequestModelDispatcher);
      }
    }

    void TryToSelectDispatcher ()
    {
      Model.TryToSelect ();

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void StyleHorizontalChangedDispatcher (string style)
    {
      OnStyleHorizontalSelected (style);
    }

    void StyleVerticalChangedDispatcher (string style)
    {
      OnStyleVerticalSelected (style);
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