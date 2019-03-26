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
            if (message.Result.IsValid) {
              // Collection Full
              if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full)) {
                // Image
                if (message.Support.Argument.Types.IsOperationCategory (Server.Models.Infrastructure.TCategory.Image)) {
                  var entityAction = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                  TDispatcher.BeginInvoke (ResponseDataDispatcher, entityAction);
                }
              }
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            Model.Cleanup ();
            RaiseChanged ();

            TDispatcher.Invoke (RequestDataDispatcher);
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.List)) {
          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            Model.Cleanup ();
            RaiseChanged ();

            TDispatcher.Invoke (RequestDataDispatcher);
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
      if (item.NotNull ()) {
        TDispatcher.BeginInvoke (ItemSelectedDispatcher, item);
      }
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
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (TEntityAction.Create (Server.Models.Infrastructure.TCategory.Image, Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full));

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseDataDispatcher (TEntityAction action)
    {
      Model.Select (action);
      RaiseChanged ();

      RefreshCollection ("ModelItemsViewSource");
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
  };
  //---------------------------//

}  // namespace