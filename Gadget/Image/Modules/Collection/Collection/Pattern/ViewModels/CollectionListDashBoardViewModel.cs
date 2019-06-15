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
using Shared.DashBoard;

using Gadget.Collection.Presentation;
using Gadget.Collection.Pattern.Models;
//---------------------------//

namespace Gadget.Collection.Pattern.ViewModels
{
  [Export ("ModuleCollectionListDashBoardViewModel", typeof (ICollectionListDashBoardViewModel))]
  public class TCollectionListDashBoardViewModel : TViewModelAware<TCollectionListDashBoardModel>, IHandleMessageInternal, ICollectionListDashBoardViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TCollectionListDashBoardViewModel (ICollectionPresentation presentation)
      : base (new TCollectionListDashBoardModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);

      m_Commit = true;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Collection)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.Board)) {
          if (message.IsAction (TInternalMessageAction.Response)) {
            if (message.Result.IsValid) {
              // Select - Summary
              if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Summary)) {
                var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                TDispatcher.BeginInvoke (SummaryResultDispatcher, entityAction);
              }
            }
          }

          // Reload
          if (message.IsAction (TInternalMessageAction.Reload)) {
            m_Commit = true;
          }
        }

        // from sibilig
        if (message.Node.IsSiblingToMe (TChild.Board)) {
          // Summary
          if (message.IsAction (TInternalMessageAction.Summary)) {
            var entityAction = Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction);
            TDispatcher.BeginInvoke (SummaryDispatcher, entityAction.Summary.Category);
          }
        }
      }
    }
    #endregion

    #region View Event
    public void OnBackCommadClicked ()
    {
      TDispatcher.Invoke (BackDispatcher);
    }

    public void OnItemClicked (TDashBoardEventArgs args)
    {
      if (args.NotNull ()) {
        TDispatcher.Invoke (BackDispatcher);
        TDispatcher.BeginInvoke (ItemClickedDispatcher, args);
      }
    }
    #endregion

    #region Dispatcher
    void BackDispatcher ()
    {
      // to Sibling
      var message = new TCollectionMessageInternal (TInternalMessageAction.Back, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Board);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void SummaryDispatcher (Server.Models.Infrastructure.TCategory category)
    {
      if (m_Commit) {
        // to parent (Select - Summary)
        var action = Server.Models.Component.TEntityAction.Create (
          category,
          Server.Models.Infrastructure.TOperation.Select,
          Server.Models.Infrastructure.TExtension.Summary
        );

        action.Summary.Select (category);

        var message = new TCollectionMessageInternal (TInternalMessageAction.Request, TChild.Board, TypeInfo);
        message.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    void SummaryResultDispatcher (Server.Models.Component.TEntityAction action)
    {
      Model.Select (action);

      if (FrameworkElementView.FindName ("DashBoardSummaryControl") is Shared.DashBoard.TDashBoardSummaryControl control) {
        control.SelectModel (action);

        m_Commit = false;
      }

      RaiseChanged ();
    }

    void ItemClickedDispatcher (TDashBoardEventArgs args)
    {
      // to Sibling
      var message = new TCollectionMessageInternal (TInternalMessageAction.Style, TypeInfo);
      message.Node.SelectRelationSibling (TChild.Board);
      message.Support.Argument.Types.HorizontalStyle.CopyFrom (args.HorizontalStyleInfo);
      message.Support.Argument.Types.VerticalStyle.CopyFrom (args.VerticalStyleInfo);

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
    bool                                    m_Commit; 
    #endregion
  };
  //---------------------------//

}  // namespace