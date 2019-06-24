/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Types;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;
using Shared.DashBoard;

using Layout.Factory.Presentation;
using Layout.Factory.Pattern.Models;
//---------------------------//

namespace Layout.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryBoardViewModel", typeof (IFactoryBoardViewModel))]
  public class TFactoryBoardViewModel : TViewModelAware<TFactoryBoardModel>, IHandleMessageInternal, IFactoryBoardViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryBoardViewModel (IFactoryPresentation presentation)
      : base (new TFactoryBoardModel ())
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
        // from Sibling
        if (message.Node.IsSiblingToMe (TChild.Board)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            var propertyName = message.Support.Argument.Args.PropertyName;

            if (propertyName.Equals ("Int4Property")) {
              TDispatcher.BeginInvoke (PropertySelectDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
            }
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestModelDispatcher, Server.Models.Component.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Drop
          if (message.IsAction (TInternalMessageAction.Drop)) {
            TDispatcher.BeginInvoke (DropFromBoardDispatcher, message.Support.Argument.Args.Id);
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
    public void OnDashBoardDropFromSource (TDashBoardEventArgs args)
    {
      // remove drop from source 
      TDispatcher.BeginInvoke (DropFromSourceDispatcher, args);

      var contentInfo = TContentInfo.CreateDefault;
      contentInfo.Select (args.Id, args.TargetPosition);
      contentInfo.Select (args.Category);

      TDispatcher.BeginInvoke (ContentInsertDispatcher, contentInfo);

      TDispatcher.BeginInvoke (ReportDispatcher, args.ReportData);
    }

    public void OnDashBoardContentMoved (TDashBoardEventArgs args)
    {
      TDispatcher.BeginInvoke (ContentMovedDispatcher, new Tuple<TPosition, TPosition> (args.SourcePosition, args.TargetPosition));
    }

    public void OnDashBoardContentRemoved (TDashBoardEventArgs args)
    {
      TDispatcher.BeginInvoke (ReportDispatcher, args.ReportData);
    }

    public void OnDashBoardControlLoaded (object control)
    {
      if (control is TDashBoardControl dashControl) {
        m_DashBoardControl = dashControl;
      }
    }
    #endregion

    #region Dispatcher
    void CleanupDispatcher ()
    {
      m_DashBoardControl.Cleanup ();
    }

    void PropertySelectDispatcher (Server.Models.Component.TEntityAction action)
    {
      // PropertyName = Int4Property (Column or Rows property changed)
      // action.ModelAction.ExtensionGeometryModel (Cols, Rows)

      action.ThrowNull ();

      int cols = action.ModelAction.ExtensionGeometryModel.SizeCols;
      int rows = action.ModelAction.ExtensionGeometryModel.SizeRows;

      m_DashBoardControl.LayoutChanged (TSize.Create (cols, rows));

      RaiseChanged ();
    }

    void ReportDispatcher (TReportData report)
    {
      // to Sibling (Report)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Report, TChild.Board, TypeInfo);
      message.Support.Argument.Types.Select (report);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void DropFromBoardDispatcher (Guid id)
    {
      TDispatcher.BeginInvoke (ContentRemovedDispatcher, id);

      m_DashBoardControl.RemoveContent (id);
    }

    void DropFromSourceDispatcher (TDashBoardEventArgs args)
    {
      // to Sibling (Drop)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Drop, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (args.Id);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    #region Content
    void ContentInsertDispatcher (TContentInfo contentInfo)
    {
      // to Sibling (Select)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Select, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (contentInfo, null);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ContentRemovedDispatcher (Guid id)
    {
      // to Sibling (Remove)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Remove, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (id);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ContentMovedDispatcher (Tuple<TPosition, TPosition> tuple)
    {
      // to Sibling (Move)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Move, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (tuple.Item1, tuple.Item2);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Request
    void RequestModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      m_DashBoardControl.RequestModel (action);

      // to Sibling (Response)
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.Board, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Edit
    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      m_DashBoardControl.SelectModel (action);
    }
    #endregion
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
    TDashBoardControl                                 m_DashBoardControl; 
    #endregion
  };
  //---------------------------//

}  // namespace