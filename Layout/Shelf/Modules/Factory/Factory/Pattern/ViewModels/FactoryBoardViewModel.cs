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
        // parent to me
        if (message.Node.IsParentToMe (TChild.Board)) {
          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Select Many
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many)) {
              if (message.Result.IsValid) {
                // bag
                //if (message.Support.Argument.Types.IsContextType (Server.Models.Infrastructure.TContextType.Context.Bag)) {
                //  TDispatcher.BeginInvoke (ResponseSelectManyDispatcher, Server.Models.Module.Bag.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
                //}

                //// slide
                //if (message.Support.Argument.Types.IsContextType (Server.Models.Infrastructure.TContextType.Context.Slide)) {
                //  TDispatcher.BeginInvoke (ResponseSelectManyDispatcher, Server.Models.Module.Slide.TEntityAction.Request (message.Support.Argument.Types.EntityAction));
                //}

                //// content
                //if (message.Support.Argument.Types.IsContextType (Server.Models.Infrastructure.TContextType.Context.Content)) {
                //  var action = Server.Models.Content.TEntityAction.Request (message.Support.Argument.Types.EntityAction);

                //  switch (action.CollectionAction.ContentOperation.Operation) {
                //    case Server.Models.Content.TContentOperation.TInternalOperation.Type:
                //      TDispatcher.BeginInvoke (ResponseContentByTypeDispatcher, action);
                //      break;

                //    case Server.Models.Content.TContentOperation.TInternalOperation.Id:
                //      TDispatcher.BeginInvoke (ResponseContentByIdDispatcher, action);
                //      break;
                //  }
                //}
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.Board)) {
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

    public void OnDashBoardSizeChanged (TDashBoardEventArgs args)
    {
      TDispatcher.BeginInvoke (SizeChangedDispatcher, args.BoardSize);
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
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Report, TChild.Board, TypeInfo);
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
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Drop, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (args.Id);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void SizeChangedDispatcher (TSize size)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Size, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (size, null);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    #region Content
    void ContentInsertDispatcher (TContentInfo contentInfo)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Select, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (contentInfo, null);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ContentRemovedDispatcher (Guid id)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Remove, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (id);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ContentMovedDispatcher (Tuple<TPosition, TPosition> tuple)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Move, TChild.Board, TypeInfo);
      message.Support.Argument.Args.Select (tuple.Item1, tuple.Item2);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Request
    void RequestModelDispatcher (Server.Models.Component.TEntityAction action)
    {
      m_DashBoardControl.RequestModel (action);

      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Response, TChild.Board, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    //void RequestComponentRelationDispatcher ()
    //{
    //  // to parent
    //  var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Relation);
    //  action.CollectionAction.SelectComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Category);
    //  action.ComponentOperation.SelectByCategory (Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Shelf));

    //  var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Board, TypeInfo);
    //  message.Support.Argument.Types.Select (action);

    //  DelegateCommand.PublishInternalMessage.Execute (message);
    //}

    //void RequestDataDispatcher ()
    //{
    //  // to parent
    //  //bag
    //  var action = Server.Models.Component.TEntityAction.Create (Server.Models.Infrastructure.TCategory.Bag, Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Zap);
    //  Model.RequestRelations (action);

    //  var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Board, TypeInfo);
    //  message.Support.Argument.Types.Select (action);

    //  TDispatcher.BeginInvoke (JustDispatcher, message);
    //}
    #endregion

    #region Response
    //void ResponseContentByIdDispatcher (Server.Models.Content.TEntityAction action)
    //{
    //  Model.SelectById (action);

    //  // bag
    //  var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Board, TypeInfo);
    //  var bagAction = Server.Models.Module.Bag.TEntityAction.Create (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);

    //  if (Model.RequestContent (Server.Models.Infrastructure.TContextType.Context.Bag, bagAction.IdCollection)) {
    //    message.Support.Argument.Types.Select (bagAction);

    //    TDispatcher.BeginInvoke (JustDispatcher, message);
    //  }

    //  // slide
    //  var msg = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Board, TypeInfo);
    //  var slideAction = Server.Models.Module.Slide.TEntityAction.Create (Server.Models.Infrastructure.TOperation.Select, Server.Models.Infrastructure.TExtension.Many);

    //  if (Model.RequestContent (Server.Models.Infrastructure.TContextType.Context.Slide, slideAction.IdCollection)) {
    //    msg.Support.Argument.Types.Select (slideAction);

    //    TDispatcher.BeginInvoke (JustDispatcher, msg);
    //  }
    //}

    //void ResponseComponentRelationDispatcher (Server.Models.Component.TEntityAction action)
    //{
    //  Model.SelectComponentRelation (action);

    //  TDispatcher.Invoke (RequestDataDispatcher);
    //}

    //void ResponseDataDispatcher (Server.Models.Component.TEntityAction action)
    //{
    //  Model.Select (action);

    //  TDispatcher.Invoke (RefreshAllCollectionDispatcher);
    //}

    //void ResponseSelectManyDispatcher (Server.Models.Module.Bag.TEntityAction action)
    //{
    //  // action.CollectionAction.ModelCollection[BagId] {Bag model}

    //  var contentInfoCollection = new Collection<TContentInfo> ();

    //  if (Model.SelectMany (action, contentInfoCollection)) {
    //    foreach (var item in contentInfoCollection) {
    //      ContentSelected (item);
    //    }

    //    TDispatcher.Invoke (NotifyReportDispatcher);
    //  }

    //  TDispatcher.Invoke (RefreshAllCollectionDispatcher);
    //}

    //void ResponseSelectManyDispatcher (Server.Models.Module.Slide.TEntityAction action)
    //{
    //  // action.CollectionAction.ModelCollection {Slide model}
    //  // action.CollectionAction.EntityCollection[SlideId].CollectionAction.ModelColletion[FrameId] {Frame model}

    //  var contentInfoCollection = new Collection<TContentInfo> ();

    //  if (Model.SelectMany (action, contentInfoCollection)) {
    //    foreach (var item in contentInfoCollection) {
    //      ContentSelected (item);
    //    }

    //    TDispatcher.Invoke (NotifyReportDispatcher);
    //  }

    //  TDispatcher.Invoke (RefreshAllCollectionDispatcher);
    //}
    #endregion

    #region Edit
    void EditDispatcher (Server.Models.Component.TEntityAction action)
    {
      /*
      action.Id // shelf Id
      action.ModelAction // shelf model
      action.Param1 {TComponentModelItem} // relation
      action.Param2  {Shared.Layout.Bag.TComponentControlModel} // child model
      */

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

    #region Support
    void ContentInsert (TContentInfo contentInfo)
    {
      TDispatcher.BeginInvoke (ContentInsertDispatcher, contentInfo);
    }

    void ContentRemoved (TDashBoardItem item)
    {
      //?????????????????????????????
      //TODO: review
      //var style = item.ContextStyle;
      var contentItem = new TDashBoardItem (item); // preserve

      //if (Model.RemoveContent (item.Id, out TPosition position)) {
        //Model.ChangeStatus (position, style, TDashBoardItem.TDashBoardStatus.Standby);


        //TDispatcher.BeginInvoke (ContentRemovedDispatcher, contentItem);
      //}
    }

    void ContentMoved (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      TDispatcher.BeginInvoke (ContentMovedDispatcher, new Tuple<TPosition, TPosition> (sourceItem.Position, targetItem.Position));
    }
    #endregion
  };
  //---------------------------//

}  // namespace