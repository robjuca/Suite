/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using rr.Library.Helper;
using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Shared.DashBoard
{
  [TemplatePart (Name = PART_DASHBOARD, Type = typeof (ItemsControl))]
  [TemplatePart (Name = PART_COLUMNSLIDE, Type = typeof (Slider))]
  [TemplatePart (Name = PART_ROWSLIDE, Type = typeof (Slider))]
  public class TDashBoardControl : Control, GongSolutions.Wpf.DragDrop.IDropTarget
  {
    #region Property
    public ObservableCollection<TDashBoardItem> DashBoardItemSource
    {
      get;
      private set;
    }

    public TSize Size
    {
      get;
    }
    #endregion

    #region Constructor
    static TDashBoardControl ()
    {
      DefaultStyleKeyProperty.OverrideMetadata (typeof (TDashBoardControl), new FrameworkPropertyMetadata (typeof (TDashBoardControl)));
    }

    public TDashBoardControl ()
    {
      DashBoardItemSource = new ObservableCollection<TDashBoardItem> ();

      // 4x4 matrix
      for (int row = 1; row <= m_MaxRow; row++) {
        for (int col = 1; col <= m_MaxColumn; col++) {
          DashBoardItemSource.Add (new TDashBoardItem (TPosition.Create (col, row)));
        }
      }

      Size = TSize.Create (m_MaxColumn, m_MaxRow); // cols = 4, rows = 4 max

      m_DashboardCollectionViewSource = new CollectionViewSource
      {
        Source = DashBoardItemSource
      };

      m_SlideSize = TSize.Create (m_MaxColumn, m_MaxRow);
    }
    #endregion

    #region Event
    // Declare the delegate
    public delegate void    DropFromSourceEventHandler (object sender, TDashBoardEventArgs e);
    public delegate void    ContentMovedEventHandler (object sender, TDashBoardEventArgs e);
    public delegate void    ContentRemovedEventHandler (object sender, TDashBoardEventArgs e);
    public delegate void    BoardSizeChangedEventHandler (object sender, TDashBoardEventArgs e);

    // Declare the event.
    public event DropFromSourceEventHandler           DropFromSource;
    public event ContentMovedEventHandler             ContentMoved;
    public event ContentMovedEventHandler             ContentRemoved;
    public event BoardSizeChangedEventHandler         BoardSizeChanged;
    #endregion

    #region IDragDrop
    void GongSolutions.Wpf.DragDrop.IDropTarget.DragOver (GongSolutions.Wpf.DragDrop.IDropInfo dropInfo)
    {
      bool canDrop = false;

      dropInfo.Effects = DragDropEffects.None;

      if (dropInfo.TargetItem is TDashBoardItem targetItem) {
        // external source
        if (dropInfo.Data is TComponentModelItem externalData) {
          canDrop = CanDragOver (externalData, targetItem);
        }

        // myself source (Board)
        if (dropInfo.Data is TDashBoardItem internalData) {
          canDrop = CanMove (internalData, targetItem);
        }
      }

      if (canDrop) {
        dropInfo.DropTargetAdorner = GongSolutions.Wpf.DragDrop.DropTargetAdorners.Highlight;
        dropInfo.Effects = DragDropEffects.Move;
      }
    }

    void GongSolutions.Wpf.DragDrop.IDropTarget.Drop (GongSolutions.Wpf.DragDrop.IDropInfo dropInfo)
    {
      if (dropInfo.TargetItem is TDashBoardItem targetItem) {
        // external source 
        if (dropInfo.Data is TComponentModelItem externalData) {
          DoDrop (externalData, targetItem);

          // remove drop from source 
          var args = TDashBoardEventArgs.CreateDefault;
          args.Id = externalData.Id;
          args.TargetPosition.CopyFrom (targetItem.Position);

          RequestReport (args);

          DropFromSource?.Invoke (this, args);
        }

        // myself source 
        if (dropInfo.Data is TDashBoardItem internalData) {
          // move
          DoMove (internalData, targetItem);

          // notify 
          var args = TDashBoardEventArgs.CreateDefault;
          args.SourcePosition.CopyFrom (internalData.Position);
          args.TargetPosition.CopyFrom (targetItem.Position);

          ContentMoved?.Invoke (this, args);
        }

        TDispatcher.Invoke (RefreshCollectionDispatcher);
      }
    }
    #endregion

    #region Members
    public void ChangeStatus (TPosition position, TSize size, TDashBoardItem.TDashBoardStatus status)
    {
      var col = position.Column; // 1, 2, 3, 4
      var row = position.Row;    // 1, 2, 3, 4

      var sizeColumns = (size.Columns <= m_MaxColumn) ? size.Columns : m_MaxColumn; // max 4
      var sizeRows = (size.Rows <= m_MaxRow) ? size.Rows : m_MaxRow; // max 4

      string background = RequestBackground (position, status);

      for (int positionColumn = 0; positionColumn < sizeColumns; positionColumn++) {
        for (int positionRow = 0; positionRow < sizeRows; positionRow++) {
          ChangeStatus (TPosition.Create ((positionColumn + col), (positionRow + row)), status, background);
        }
      }

      SlideSetup ();
    }

    public void LayoutChanged (TSize size)
    {
      if (size.IsEmpty.IsFalse ()) {
        if (Size.IsSize (size).IsFalse ()) {
          Size.CopyFrom (size);

          if (IsDashBoardEmpty ()) {
            CleanupDashBoard ();

            for (int col = m_MaxColumn; col > Size.Columns; col--) {
              foreach (var item in DashBoardItemSource) {
                item.DisableByColumn (col);
              }
            }

            for (int row = m_MaxRow; row > size.Rows; row--) {
              foreach (var item in DashBoardItemSource) {
                item.DisableByRow (row);
              }
            }
          }

          var args = TDashBoardEventArgs.CreateDefault;
          args.BoardSize.CopyFrom (Size);

          BoardSizeChanged?.Invoke (this, args);

          SlideSetup ();
        }
      }

      TDispatcher.Invoke (RefreshCollectionDispatcher);
    }

    public bool RemoveContent (Guid id)
    {
      bool res = false;

      var dashboardItem = Select (id);

      if (dashboardItem.NotNull ()) {
        var args = TDashBoardEventArgs.CreateDefault;
        args.Id = id;
        args.SourcePosition.CopyFrom (dashboardItem.Position);
        args.TargetPosition.CopyFrom (dashboardItem.Position);

        ChangeStatusToStandby (dashboardItem);
        res = true;

        RequestReport (args);

        ContentRemoved?.Invoke (this, args);

        SlideSetup ();
      }

      TDispatcher.Invoke (RefreshCollectionDispatcher);

      return (res);
    }

    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      var componentModelItemCollection = new Collection<TComponentModelItem> (); // component model 

      if (action.Param1 is Collection<TComponentModelItem> relationModels) {
        componentModelItemCollection = new Collection<TComponentModelItem> (relationModels); // relation model
      }

      foreach (var item in componentModelItemCollection) {
        var horizontalStyle = TContentStyle.TryToParse (item.LayoutModel.StyleHorizontal);
        var verticalStyle = TContentStyle.TryToParse (item.LayoutModel.StyleVertical);

        // update size
        var contentStyle = TContentStyle.CreateDefault;

        item.GeometryModel.SizeCols = contentStyle.RequestBoardStyleSize (horizontalStyle);
        item.GeometryModel.SizeRows = contentStyle.RequestBoardStyleSize (verticalStyle);

        var dashBoardItem = Select (item.Position);

        if (dashBoardItem.NotNull ()) {
          dashBoardItem.SelectModel (item);

          TDispatcher.BeginInvoke (ChangeStatusToBusyDispatcher, dashBoardItem);
        }
      }

      TDispatcher.Invoke (RefreshCollectionDispatcher);

      m_SlideStatusDisable = true;

      SlideSetup ();
    }

    public void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      // size 
      var contentStyle = TContentStyle.CreateDefault;
      contentStyle.RequestSize (Size);

      action.ModelAction.ExtensionGeometryModel.SizeCols = Size.Columns;
      action.ModelAction.ExtensionGeometryModel.SizeRows = Size.Rows;

      action.ModelAction.ExtensionLayoutModel.Width = Size.Width;
      action.ModelAction.ExtensionLayoutModel.Height = Size.Height;

      action.ModelAction.ExtensionLayoutModel.StyleHorizontal = string.Empty;
      action.ModelAction.ExtensionLayoutModel.StyleVertical = string.Empty;

      foreach (var item in DashBoardItemSource) {
        if (item.IsBusy) {
          if (item.Id.NotEmpty ()) {
            var relation = Server.Models.Component.ComponentRelation.CreateDefault;
            relation.ChildId = item.Id;
            relation.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (item.Category);
            relation.PositionColumn = item.Position.Column;
            relation.PositionRow = item.Position.Row;
            relation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

            action.CollectionAction.ComponentRelationCollection.Add (relation);
          }
        }
      }
    }

    public void Cleanup ()
    {
      CleanupDashBoard ();

      LayoutChanged (TSize.Create (m_MaxColumn, m_MaxRow));
    }
    #endregion

    #region Drag Operation
    public bool CanDragOver (TComponentModelItem sourceItem, TDashBoardItem targetItem)
    {
      bool canDrop = false;

      var col = targetItem.Position.Column; // 1, 2, 3, 4
      var row = targetItem.Position.Row;    // 1, 2, 3, 4

      sourceItem.RequestSize ();

      var sizeColumns = sourceItem.GeometryModel.SizeCols <= m_MaxColumn ? sourceItem.GeometryModel.SizeCols : m_MaxColumn; // max 4
      var sizeRows = sourceItem.GeometryModel.SizeRows <= m_MaxRow ? sourceItem.GeometryModel.SizeRows : m_MaxRow; // max 4

      if (((col - 1) + sizeColumns) <= Size.Columns && ((row - 1) + sizeRows) <= Size.Rows) { // zero index
        for (int positionColumn = 0; positionColumn < sizeColumns; positionColumn++) {
          for (int positionRow = 0; positionRow < sizeRows; positionRow++) {
            canDrop = IsStandbyByPosition (TPosition.Create ((positionColumn + col), (positionRow + row)));

            if (canDrop.IsFalse ()) {
              return (false);
            }
          }
        }
      }

      return (canDrop);
    }

    public bool CanMove (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      // same position is forbidden
      if (targetItem.IsPosition (sourceItem)) {
        return (false);
      }

      // must be root
      if (sourceItem.IsRoot.IsFalse ()) {
        return (false);
      }

      // request room
      if (RequestStandbyRoom (sourceItem, targetItem)) {
        return (true);
      }

      //// same column
      //if (ZapSameColumn (sourceItem, targetItem)) {
      //  return (true);
      //}

      //// same row
      //if (ZapSameRow (sourceItem, targetItem)) {
      //  return (true);
      //}

      //// different col and row
      //if (ZapDifferentColRow (sourceItem, targetItem)) {
      //  return (true);
      //}

      return (false);
    }

    public void DoDrop (TComponentModelItem sourceItemModel, TDashBoardItem targetItem)
    {
      targetItem.SelectModel (sourceItemModel);

      ChangeStatus (targetItem.Position, sourceItemModel.Size, TDashBoardItem.TDashBoardStatus.Busy);
    }

    public void DoMove (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      // create alias
      var sourceAlias = new TDashBoardItem (sourceItem);
      var targetAlias = new TDashBoardItem (targetItem);

      // select items
      var source = Select (sourceItem);
      var target = Select (targetItem);

      // cleanup status
      ChangeStatusToStandby (source);
      ChangeStatusToStandby (target);

      // do move
      source.CopyFrom (targetAlias, preservePosition: true);
      target.CopyFrom (sourceAlias, preservePosition: true);

      // update status
      ChangeStatus (source);
      TDispatcher.BeginInvoke (ChangeStatusDispatcher, target);

      TDispatcher.Invoke (RefreshCollection);
    }
    #endregion

    #region Dispatcher
    void RefreshCollectionDispatcher ()
    {
      RefreshCollection ();
    }

    void ChangeStatusDispatcher (TDashBoardItem item)
    {
      ChangeStatus (item);
    }

    void ChangeStatusToBusyDispatcher (TDashBoardItem item)
    {
      ChangeStatusToBusy (item);
    }
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      /*
       dragdrop:DragDrop.IsDragSource="True"
       dragdrop:DragDrop.IsDropTarget="True"
       dragdrop:DragDrop.DropHandler="{Binding}"
       dragdrop:DragDrop.UseDefaultEffectDataTemplate="True"
       dragdrop:DragDrop.UseDefaultDragAdorner="True"
     */

      base.OnApplyTemplate ();

      if (GetTemplateChild (PART_DASHBOARD) is ItemsControl list) {
        list.ItemsSource = m_DashboardCollectionViewSource.View;

        list.SetValue (GongSolutions.Wpf.DragDrop.DragDrop.DropHandlerProperty, this);
      }

      // slide
      if (GetTemplateChild (PART_COLUMNSLIDE) is Slider columnSlide) {
        m_ColumnSlide = columnSlide;
        m_ColumnSlide.ValueChanged += OnColumnSlideValueChanged;
      }

      if (GetTemplateChild (PART_ROWSLIDE) is Slider rowSlide) {
        m_RowSlide = rowSlide;
        m_RowSlide.ValueChanged += OnRowSlideValueChanged;
      }

      SlideSetup ();
    }
    #endregion

    #region Event
    void OnColumnSlideValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      m_SlideSize.SelectColumns ((int) e.NewValue);

      if (e.NewValue.Equals (0)) {
        m_SlideSize.SelectColumns (1);
        m_ColumnSlide.Value = 1;
      }

      LayoutChanged (m_SlideSize);
    }

    void OnRowSlideValueChanged (object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      m_SlideSize.SelectRows ((int) e.NewValue);

      if (e.NewValue.Equals (0)) {
        m_SlideSize.SelectRows (1);
        m_RowSlide.Value = 1;
      }

      LayoutChanged (m_SlideSize);
    }
    #endregion

    #region Fields
    Slider                                                      m_ColumnSlide;
    Slider                                                      m_RowSlide;
    readonly TSize                                              m_SlideSize;
    readonly CollectionViewSource                               m_DashboardCollectionViewSource;
    const int                                                   m_MaxColumn = 4;
    const int                                                   m_MaxRow = 4;
    bool                                                        m_SlideStatusDisable;
    #endregion

    #region Static
    const string PART_DASHBOARD                       = "PART_DashBoard";
    const string PART_COLUMNSLIDE                     = "PART_ColumnSlide";
    const string PART_ROWSLIDE                        = "PART_RowSlide";
    #endregion

    #region Support
    void RefreshCollection ()
    {
      m_DashboardCollectionViewSource.View.Refresh ();
    }

    void CleanupDashBoard ()
    {
      foreach (var item in DashBoardItemSource) {
        item.Cleanup ();
      }
    }

    bool IsDashBoardEmpty ()
    {
      foreach (var item in DashBoardItemSource) {
        if (item.IsBusy) {
          return (false);
        }
      }

      return (true);
    }

    bool IsDashBoardBusy ()
    {
      foreach (var item in DashBoardItemSource) {
        if (item.IsBusy) {
          return (true);
        }
      }

      return (false);
    }

    void RequestReport (TDashBoardEventArgs args)
    {
      if (IsDashBoardEmpty ()) {
        args.ReportData.SelectUnlock ();
      }

      if (IsDashBoardBusy ()) {
        args.ReportData.SelectLock ();
      }
    }

    protected bool IsStandbyByPosition (TPosition position)
    {
      var item = Select (position);

      return (item.IsNull () ? false : item.IsStandby);
    }

    TDashBoardItem Select (TPosition position)
    {
      foreach (var item in DashBoardItemSource) {
        if (item.IsPosition (position)) {
          return (item);
        }
      }

      return (null);
    }

    TDashBoardItem Select (TDashBoardItem alias)
    {
      // busy
      if (alias.IsBusy) {
        return (Select (alias.Id));
      }

      // standby
      return (Select (alias.Position));
    }

    TDashBoardItem Select (Guid contextId)
    {
      foreach (var item in DashBoardItemSource) {
        if (item.Id.Equals (contextId)) {
          return (item);
        }
      }

      return (null);
    }

    string RequestBackground (TPosition position, TDashBoardItem.TDashBoardStatus status)
    {
      var background = "#ffffff";

      if (status.Equals (TDashBoardItem.TDashBoardStatus.Standby)) {
        return (background);
      }

      var boardItem = Select (position);

      if (boardItem.NotNull ()) {
        boardItem.RequestBackground ();
        background = boardItem.Background;
      }

      return (background);
    }

    void ChangeStatus (TPosition position, TDashBoardItem.TDashBoardStatus status, string background)
    {
      var boardItem = Select (position);

      if (boardItem.NotNull ()) {
        boardItem.SelectBackground (background);
        boardItem.ChangeStatus (status);
      }
    }

    void ChangeStatusToStandby (TDashBoardItem alias)
    {
      ChangeStatus (alias.Position, alias.Size, TDashBoardItem.TDashBoardStatus.Standby);
    }

    void ChangeStatusToBusy (TDashBoardItem alias)
    {
      m_SlideStatusDisable = true;

      ChangeStatus (alias.Position, alias.Size, TDashBoardItem.TDashBoardStatus.Busy);
    }

    void ChangeStatus (TDashBoardItem alias)
    {
      ChangeStatus (alias.Position, alias.Size, alias.DahBoardStatus);
    }

    bool RequestRoomSameRowStandby (TDashBoardItem source, TDashBoardItem target)
    {
      // target standby only
      if (target.IsBusy) {
        return (false);
      }

      var res = false;

      //switch (source.ContextStyle) {
      //  case "small":
      //    if (targetRow < 3) {
      //      var c1 = Select (TPosition.Create (targetCol, targetRow));
      //      var c2 = Select (TPosition.Create (targetCol, (targetRow + 1)));
      //      res = c1.IsStandby && c2.IsStandby;
      //    }

      //    break;

      //  case "large":
      //    if (targetRow.Equals (1)) {
      //      var c1 = Select (TPosition.Create (targetCol, targetRow));
      //      var c2 = Select (TPosition.Create (targetCol, (targetRow + 1)));
      //      var c3 = Select (TPosition.Create (targetCol, (targetRow + 2)));
      //      res = c1.IsStandby && c2.IsStandby && c3.IsStandby;
      //    }

      //    break;

      //  case "big":
      //    if ((targetCol < 4) && targetRow.Equals (1)) {
      //      // col
      //      var c1 = Select (TPosition.Create (targetCol, targetRow));
      //      var c2 = Select (TPosition.Create (targetCol, (targetRow + 1)));
      //      var c3 = Select (TPosition.Create (targetCol, (targetRow + 2)));

      //      // col + 1
      //      var c4 = Select (TPosition.Create ((targetCol + 1), targetRow));
      //      var c5 = Select (TPosition.Create ((targetCol + 1), (targetRow + 1)));
      //      var c6 = Select (TPosition.Create ((targetCol + 1), (targetRow + 2)));

      //      res = c1.IsStandby
      //        && c2.IsStandby
      //        && c3.IsStandby
      //        && c4.IsStandby
      //        && c5.IsStandby
      //        && c6.IsStandby
      //      ;
      //    }
      //    break;
      //}

      return (res);
    }

    bool RequestStandbyRoom (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      var cols = sourceItem.Size.Columns; // (1,2,3,4)
      var rows = sourceItem.Size.Rows;    // (1,2,3,4)

      var targetPositionCol = targetItem.Position.Column;
      var targetPositionRow = targetItem.Position.Row;

      if ((cols <= m_MaxColumn) && (rows <= m_MaxRow)) {
        for (int col = 0; col < cols; col++) {
          for (int row = 0; row < rows; row++) {
            var position = TPosition.Create ((col + targetPositionCol), (row + targetPositionRow));
            var item = Select (position);

            if (item.NotNull ()) {
              // inbound only
              if (IsInbound (item).IsFalse ()) {
                return (false);
              }

              // only for targetItem standby
              if (item.IsBusy) {
                return (false);
              }
            }

            else {
              return (false);
            }
          }
        }
      }

      else {
        return (false);
      }

      return (true);
    }

    bool ZapSameColumn (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      // only apply to mini (c:1 r:1) and small (c:1 r:2)
      if (targetItem.Position.Column.Equals (sourceItem.Position.Column)) {
        // only for target standby
        if (targetItem.IsStandby) {
          // c:1 r:1 (mini)
          if (sourceItem.Size.IsSize (1, 1)) {
            if (targetItem.Position.Row.Equals (1) || targetItem.Position.Row.Equals (3)) {
              return (true);
            }
          }

          // c:1 r:2 (small)
          if (sourceItem.Size.IsSize (1, 2)) {
            if (targetItem.Position.Row.Equals (1) || targetItem.Position.Row.Equals (2)) {
              return (true);
            }
          }
        }
      }

      return (false);
    }

    bool ZapSameRow (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      //different size only (small large big)
      if (targetItem.Position.Row.Equals (sourceItem.Position.Row)) {
        if (sourceItem.Size.IsSize (targetItem.Size).IsFalse ()) {
          // target standby
          if (targetItem.IsStandby) {
            if (RequestRoomSameRowStandby (sourceItem, targetItem)) {
              return (true);
            }
          }

          // target busy
          if (targetItem.IsBusy) {
            //?????????????????
          }
        }
      }

      return (false);
    }

    bool ZapDifferentColRow (TDashBoardItem sourceItem, TDashBoardItem targetItem)
    {
      if (targetItem.Position.Column.NotEquals (sourceItem.Position.Column) && targetItem.Position.Row.NotEquals (sourceItem.Position.Row)) {
        // same style
        if (targetItem.IsSameStyle (sourceItem)) {
          return (true);
        }

        // target standby (source small only)
        if (targetItem.IsStandby) {
          if (sourceItem.ContainsStyle (TContentStyle.Style.small, TContentStyle.Style.small)) {
            // check for room
            var targetCol = targetItem.Position.Column;
            var targetRow = targetItem.Position.Row;

            if (targetRow < 3) {
              var c1 = Select (TPosition.Create (targetCol, targetRow));
              var c2 = Select (TPosition.Create (targetCol, (targetRow + 1)));

              return (c1.IsStandby && c2.IsStandby);
            }
          }
        }
      }

      return (false);
    }

    bool IsInbound (TDashBoardItem item)
    {
      return ((item.Position.Column <= Size.Columns) && (item.Position.Row <= Size.Rows));
    }

    void SlideSetup ()
    {
      m_SlideSize.CopyFrom (Size);

      if (m_ColumnSlide.NotNull () && m_RowSlide.NotNull ()) {
        m_ColumnSlide.IsEnabled = IsDashBoardEmpty ();
        m_ColumnSlide.Value = Size.Columns;

        m_RowSlide.IsEnabled = IsDashBoardEmpty ();
        m_RowSlide.Value = Size.Rows;

        if (m_SlideStatusDisable) {
          m_SlideStatusDisable = false;
          m_ColumnSlide.IsEnabled = false;
          m_RowSlide.IsEnabled = false;
        }
      }
    }
    #endregion
  }
  //---------------------------//

}  // namespace