/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;

using Gadget.Collection.Presentation;
using Gadget.Collection.Pattern.Models;
//---------------------------//

namespace Gadget.Collection.Pattern.ViewModels
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
        // from parent
        if (message.Node.IsParentToMe (TChild.Display)) {
          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Remove
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Remove)) {
              if (message.Result.IsValid) {
                Model.Cleanup ();
                RaiseChanged ();

                // notify List            
                TDispatcher.Invoke (ReloadDispatcher);
              }
            }
          }
        }

        // from sibilig
        if (message.Node.IsSibilingToMe (TChild.Display)) {
          // Select
          if (message.IsAction (TInternalMessageAction.Select)) {
            TDispatcher.BeginInvoke (SelectDispatcher, message.Support.Argument.Types.Item);
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            TDispatcher.Invoke (CleanupDispatcher);
          }

          // FilterEnter
          if (message.IsAction (TInternalMessageAction.FilterEnter)) {
            Model.IsViewEnabled = false;
            RaiseChanged ();
          }

          // FilterLeave
          if (message.IsAction (TInternalMessageAction.FilterLeave)) {
            Model.IsViewEnabled = true;
            RaiseChanged ();
          }
        }
      }
    }
    #endregion

    #region View Event
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
    void SelectDispatcher (TComponentModelItem item)
    {
      Model.Select (item);

      if (FrameworkElementView.FindName ("DisplayControl") is Shared.Gadget.Document.TComponentDisplayControl control) {
        control.RefreshDesign ();
      }

      RaiseChanged ();
    }

    void EditDispatcher ()
    {
      var action = Server.Models.Component.TEntityAction.CreateDefault;
      Model.RequestModel (action);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Edit, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RemoveDispatcher ()
    {
      var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Document, Server.Models.Infrastructure.TOperation.Remove);
      Model.RequestModel (action);

      // to parent
      var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);
      //message.Support.Argument.Types.Item.ContentLocked = Model.ComponentModelItem.ContentLocked;

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ReloadDispatcher ()
    {
      // to sibiling
      var message = new TCollectionSibilingMessageInternal (TInternalMessageAction.Reload, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void CleanupDispatcher ()
    {
      Model.Cleanup ();
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
  };
  //---------------------------//

}  // namespace