/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using rr.Library.Types;
//---------------------------//

namespace Shared.DashBoard
{
  [TemplatePart (Name = PART_DASHBOARDSUMMARY, Type = typeof (ItemsControl))]
  public class TDashBoardSettingsControl : Control
  {
    #region Property
    public ObservableCollection<TDashBoardSettingsItem> DashBoardItemSource
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
    static TDashBoardSettingsControl ()
    {
      DefaultStyleKeyProperty.OverrideMetadata (typeof (TDashBoardSettingsControl), new FrameworkPropertyMetadata (typeof (TDashBoardSettingsControl)));
    }

    public TDashBoardSettingsControl ()
    {
      DashBoardItemSource = new ObservableCollection<TDashBoardSettingsItem> ();

      // 4x4 matrix
      for (int row = 1; row <= m_MaxRow; row++) {
        for (int col = 1; col <= m_MaxColumn; col++) {
          var item = TDashBoardSettingsItem.Create (TPosition.Create (col, row));

          DashBoardItemSource.Add (item);
        }
      }

      Size = TSize.Create (m_MaxColumn, m_MaxRow); // cols = 4, rows = 4 max

      m_DashboardCollectionViewSource = new CollectionViewSource
      {
        Source = DashBoardItemSource
      };
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
    #endregion
  }
  //---------------------------//

}  // namespace