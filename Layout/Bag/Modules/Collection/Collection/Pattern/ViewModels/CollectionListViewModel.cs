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

using Module.Collection.Presentation;
using Module.Collection.Pattern.Models;
//---------------------------//

namespace Module.Collection.Pattern.ViewModels
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
        if (message.Node.IsSibilingToMe (TChild.List)) {
          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            TDispatcher.Invoke (ReloadDispatcher);
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnStyleSelected (string style)
    {
      Enum.TryParse (style, out TContentStyle.Style selectedStyle);

      Model.SelectStyle (selectedStyle);
      RefreshCollection ();

      var modelItem = TComponentModelItem.CreateDefault;
      modelItem.LayoutModel.Style = style;

      if (Model.Current.NotNull ()) {
        modelItem.CopyFrom (Model.Current);
      }

      // to sibiling display
      var message = new TCollectionSibilingMessageInternal (TInternalMessageAction.PropertySelect, TChild.List, TypeInfo);
      message.Support.Argument.Args.Select ("StyleProperty");
      message.Support.Argument.Types.Item.CopyFrom (modelItem);

      DelegateCommand.PublishInternalMessage.Execute (message);

      if (Model.IsEmpty) {
        // to sibiling display
        message = new TCollectionSibilingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    public void OnSelectionChanged (object item)
    {
      if (item is TItemInfo info) {
        TDispatcher.BeginInvoke (ItemSelectedDispatcher, info.Model);
      }
    }
    #endregion

    #region Dispatcher
    void ReloadDispatcher ()
    {
      // to sibiling display
      var message = new TCollectionSibilingMessageInternal (TInternalMessageAction.Cleanup, TChild.List, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);

      Model.Cleanup ();
      RefreshCollection ();

      TDispatcher.Invoke (RequestDataDispatcher);
    }

    void RequestDataDispatcher ()
    {
      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (TEntityAction.Create (Server.Models.Infrastructure.TCategory.Bag, Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      Model.Select (action);
      RefreshCollection ();
    }

    void ItemSelectedDispatcher (TComponentModelItem item)
    {
      // to sibiling display
      var message = new TCollectionSibilingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
      message.Support.Argument.Types.Item.CopyFrom (item);

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

    #region Support
    void RefreshCollection ()
    {
      RefreshCollection ("ModelItemsViewSource");
      RaiseChanged ();
    } 
    #endregion
  };
  //---------------------------//

}  // namespace