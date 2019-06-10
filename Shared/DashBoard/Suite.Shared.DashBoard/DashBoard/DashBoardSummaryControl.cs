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

using rr.Library.Types;
using rr.Library.Helper;
//---------------------------//

namespace Shared.DashBoard
{
  [TemplatePart (Name = PART_DASHBOARDSUMMARY, Type = typeof (ItemsControl))]
  public class TDashBoardSummaryControl : Control
  {
    #region Property
    public ObservableCollection<TDashBoardSummaryItem> DashBoardItemSource
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
    static TDashBoardSummaryControl ()
    {
      DefaultStyleKeyProperty.OverrideMetadata (typeof (TDashBoardSummaryControl), new FrameworkPropertyMetadata (typeof (TDashBoardSummaryControl)));
    }

    public TDashBoardSummaryControl ()
    {
      DashBoardItemSource = new ObservableCollection<TDashBoardSummaryItem> ();

      // 4x4 matrix
      for (int row = 1; row <= m_MaxRow; row++) {
        for (int col = 1; col <= m_MaxColumn; col++) {
          DashBoardItemSource.Add (TDashBoardSummaryItem.Create (TPosition.Create (col, row)));
        }
      }

      Size = TSize.Create (m_MaxColumn, m_MaxRow); // cols = 4, rows = 4 max

      m_DashboardCollectionViewSource = new CollectionViewSource
      {
        Source = DashBoardItemSource
      };
    }
    #endregion

    #region Members
    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var item in DashBoardItemSource) {
        item.SelectModel (action);
      }

      TDispatcher.Invoke (RefreshCollectionDispatcher);
    }

    public void Cleanup ()
    {
      CleanupDashBoard ();
    }
    #endregion

    #region Dispatcher
    void RefreshCollectionDispatcher ()
    {
      RefreshCollection ();
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

      if (GetTemplateChild (PART_DASHBOARDSUMMARY) is ItemsControl list) {
        list.ItemsSource = m_DashboardCollectionViewSource.View;
      }
    }
    #endregion

    #region Fields
    readonly CollectionViewSource                               m_DashboardCollectionViewSource;
    const int                                                   m_MaxColumn = 4;
    const int                                                   m_MaxRow = 4;
    #endregion

    #region Static
    const string PART_DASHBOARDSUMMARY                          = "PART_DashBoardSummary";
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

    TDashBoardSummaryItem Select (TPosition position)
    {
      foreach (var item in DashBoardItemSource) {
        if (item.IsPosition (position)) {
          return (item);
        }
      }

      return (null);
    }
    #endregion
  }
  //---------------------------//

}  // namespace