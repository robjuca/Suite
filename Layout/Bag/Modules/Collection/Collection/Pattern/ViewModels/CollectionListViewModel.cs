/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Component;

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
            // Collection - Full
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
              if (message.Result.IsValid) {
                // Bag
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Bag)) {
                  var entityAction = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                }
              }
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            TDispatcher.Invoke (ReloadDispatcher);
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.List)) {
          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            TDispatcher.Invoke (ReloadDispatcher);
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

      NotifyStyleChanged (TContentStyle.Mode.Horizontal);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    public void OnStyleVerticalSelected (string style)
    {
      Enum.TryParse (style, out TContentStyle.Style selectedStyle);
      Model.SelectStyleVertical (selectedStyle);

      NotifyStyleChanged (TContentStyle.Mode.Vertical);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    public void OnSelectionChanged (object item)
    {
      if (item is TComponentModelItem modelItem) {
        TDispatcher.BeginInvoke (ItemSelectedDispatcher, modelItem);
      }
    }

    public void OnDashBoardClicked ()
    {
      Model.SlideIndex = 2;
      TDispatcher.Invoke (RefreshAllDispatcher);

      //to Sibling
      var action = Server.Models.Component.TEntityAction.CreateDefault;
      action.Summary.Select (Server.Models.Infrastructure.TCategory.Bag);

      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Summary, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RefreshCollection ("ModelItemsViewSource");

      RaiseChanged ();
    }

    void ReloadDispatcher ()
    {
      // to Sibling display
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);

      Model.Cleanup ();

      TDispatcher.Invoke (RequestDataDispatcher);
    }

    void RequestDataDispatcher ()
    {
      // to parent (Collection - Full)
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (TEntityAction.Create (Server.Models.Infrastructure.TCategory.Bag, Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      Model.Select (action);

      TDispatcher.Invoke (RefreshAllDispatcher);
    }

    void ItemSelectedDispatcher (TComponentModelItem item)
    {
      // to Sibling display
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (item);

      DelegateCommand.PublishInternalMessage.Execute (message);

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

    #region Support
    void NotifyStyleChanged (TContentStyle.Mode styleMode)
    {
      var propertyName = styleMode.Equals (TContentStyle.Mode.Horizontal) ? "StyleHorizontalProperty" : styleMode.Equals (TContentStyle.Mode.Vertical) ? "StyleVerticalProperty" : string.Empty;

      var modelItem = TComponentModelItem.CreateDefault;
      modelItem.CopyFrom (Model.Current);

      // to Sibling display
      var message = new TCollectionSiblingMessageInternal (TInternalMessageAction.PropertySelect, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select (propertyName);
      message.Support.Argument.Types.Item.CopyFrom (modelItem);

      DelegateCommand.PublishInternalMessage.Execute (message);

      if (Model.IsEmpty) {
        // to Sibling display
        message = new TCollectionSiblingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace