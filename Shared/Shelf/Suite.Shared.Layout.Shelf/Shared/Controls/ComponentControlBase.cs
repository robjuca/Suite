﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using rr.Library.Types;

using Shared.ViewModel;
using Shared.Types;
//---------------------------//

namespace Shared.Layout.Shelf
{
  public abstract class TComponentControlBase : Border
  {
    #region Dependency Property
    public static readonly DependencyProperty ModelProperty =
      DependencyProperty.Register ("Model", typeof (TComponentControlModel), typeof (TComponentControlBase),
      new FrameworkPropertyMetadata (TComponentControlModel.CreateDefault, ModelPropertyChanged));
    #endregion

    #region Property
    public TComponentControlModel Model
    {
      get
      {
        return (TComponentControlModel) GetValue (ModelProperty);
      }

      set
      {
        SetValue (ModelProperty, value);
      }
    }

    public Guid Id
    {
      get
      {
        return (ControlModelMode.Equals (TControlModelMode.Default) ? Model.Id : ControlModelMode.Equals (TControlModelMode.Local) ? ModelLocal.Id : Guid.Empty);
      }
    }
    #endregion

    #region Constructor
    TComponentControlBase ()
    {
      HorizontalAlignment = HorizontalAlignment.Left;
      VerticalAlignment = VerticalAlignment.Top;

      m_ContentItems = new Collection<TContentItemModel> ();

      ControlMode = TControlMode.None;
      ControlModelMode = TControlModelMode.None;

      ModelLocal = TComponentControlModel.CreateDefault;
    }

    protected TComponentControlBase (TControlMode mode)
      : this ()
    {
      ControlMode = mode;
      ControlModelMode = TControlModelMode.Default;
    }

    protected TComponentControlBase (TControlMode mode, TComponentControlModel model)
      : this ()
    {
      ControlMode = mode;
      ControlModelMode = TControlModelMode.Local;

      ModelLocal.CopyFrom (model);
    }
    #endregion

    #region Members
    public void ChangeSize (TSize size)
    {
      SizeCols = size.Columns;
      SizeRows = size.Rows;
    }

    public void InsertContent (IList<TComponentModelItem> contentCollection)
    {
      RefreshDesign (); // create container

      // contentCollection contains only Bag
      if (contentCollection.NotNull ()) {
        foreach (var content in contentCollection) {
          var id = content.Id;
          var category = content.Category;
          var position = content.Position;

          var controlModel = Shared.Layout.Bag.TComponentControlModel.CreateDefault;
          controlModel.SelectModel (id, category);

          // Bag contains only one child (Document or Image or Video)
          if (content.ChildCollection.Count.Equals (1)) {
            var childModel = content.ChildCollection [0];
            var childId = childModel.Id;
            var childCategory = childModel.Category;
            //TODO: review
            //var childStyle = childModel.Style;

            //controlModel.SelectChildModel (childId, childCategory, childStyle, childModel);
          }

          InsertContent (position, controlModel);
        }
      }
    }

    public void InsertContent (TPosition position, Shared.Layout.Bag.TComponentControlModel model)
    {
      m_ContentItems.Add (new TContentItemModel (position, model));

      InsertChild (position, model);

      if (ControlMode.Equals (TControlMode.Display)) {
        UpdateLayout (position);
      }
    }

    public void ChangeContent (Guid id, TComponentModelItem item)
    {
      //int index = SelectByIndex (bagId);

      //if (index > -1) {
      //  m_ContentItems [index].BagItem.CopyFrom (bagItem);

      //  for (index = 0; index < m_ContentContainer.Children.Count; index++) {
      //    var border = m_ContentContainer.Children [index] as Border;

      //    if (border.Child is Bag.TDisplayControl child) {
      //      if (child.Id.Equals (bagId)) {
      //        child.Model = CreateModel (bagItem);
      //        child.RefreshDesign ();
      //        break;
      //      }
      //    }
      //  }
      //}
    }

    public void RemoveContent (Guid contentId)
    {
      int index = SelectByIndex (contentId);

      if (index > -1) {
        m_ContentItems.RemoveAt (index);
        RemoveChild (contentId);
      }
    }

    public void DoMove (TPosition sourcePosition, TPosition targetPosition)
    {
      var source = Select (sourcePosition);
      var target = Select (targetPosition);

      RemoveChild (source);
      RemoveChild (target);

      ChangePosition (targetPosition, source);
      ChangePosition (sourcePosition, target);

      InsertChild (source);
      InsertChild (target);

      if (ControlMode.Equals (TControlMode.Display) && source.NotNull ()) {
        UpdateLayout (source.Position);
      }

      if (ControlMode.Equals (TControlMode.Display) && target.NotNull ()) {
        UpdateLayout (target.Position);
      }
    }

    public void Cleanup ()
    {
      Collection<Guid> ids = new Collection<Guid> ();

      foreach (var item in m_ContentItems) {
        ids.Add (item.Id);
      }

      foreach (var id in ids) {
        RemoveContent (id);
      }
    }

    public void RefreshDesign ()
    {
      SizeCols = Model.EntityAction.ModelAction.ExtensionGeometryModel.SizeCols;
      SizeRows = Model.EntityAction.ModelAction.ExtensionGeometryModel.SizeRows;

      CreateContentContainer ();

      Child = m_ContentContainer;
    }
    #endregion

    #region Callback
    static void ModelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TComponentControlBase control) {
        if (e.NewValue is TComponentControlModel) {
          control.RefreshDesign ();
        }
      }
    }
    #endregion

    #region Property
    TControlMode ControlMode
    {
      get;
      set;
    }

    TControlModelMode ControlModelMode
    {
      get;
      set;
    }

    TComponentControlModel ModelLocal
    {
      get;
      set;
    }

    public int SizeCols
    {
      get;
      private set;
    }

    public int SizeRows
    {
      get;
      private set;
    }
    #endregion

    #region Fields
    public Grid                                       m_ContentContainer;
    public Collection<TContentItemModel>              m_ContentItems;
    #endregion

    #region Virtual
    public virtual void CreateContentContainer ()
    {
    }

    public virtual void RequestBorder (Border border)
    {
    }
    #endregion

    #region Support
    void InsertChild (TPosition position, Shared.Layout.Bag.TComponentControlModel model)
    {
      var displayControl = new Shared.Layout.Bag.TComponentDisplayControl (model);

      var border = new Border ()
      {
        Background = Brushes.White,
        Child = displayControl,
      };

      RequestBorder (border);

      var col = position.Column;
      var row = position.Row;

      switch (model.Style) {
        case "mini":
          border.SetValue (Grid.ColumnProperty, (col - 1));
          border.SetValue (Grid.RowProperty, (row - 1));
          break;

        case "small":
          border.SetValue (Grid.ColumnProperty, (col - 1));
          border.SetValue (Grid.RowProperty, (row - 1));
          border.SetValue (Grid.RowSpanProperty, 2);
          break;

        case "large":
          border.SetValue (Grid.ColumnProperty, (col - 1));
          border.SetValue (Grid.RowProperty, (row - 1));
          border.SetValue (Grid.RowSpanProperty, 3);
          break;

        case "big":
          border.SetValue (Grid.ColumnProperty, (col - 1));
          border.SetValue (Grid.ColumnSpanProperty, 2);
          border.SetValue (Grid.RowSpanProperty, 3);
          break;
      }

      m_ContentContainer.Children.Add (border);

      displayControl.Refresh ();
    }

    void InsertChild (TContentItemModel item)
    {
      if (item.NotNull ()) {
        InsertChild (item.Position, item.ComponentControlModel);
      }
    }

    void RemoveChild (Guid contentId)
    {
      if (contentId.NotEmpty ()) {
        for (int index = 0; index < m_ContentContainer.Children.Count; index++) {
          var border = m_ContentContainer.Children [index] as Border;

          if (border.Child is Shared.Layout.Bag.TComponentDisplayControl bagChild) {
            if (bagChild.Id.Equals (contentId)) {
              m_ContentContainer.Children.RemoveAt (index);
              break;
            }
          }
        }
      }
    }

    void RemoveChild (TContentItemModel item)
    {
      if (item.NotNull ()) {
        RemoveChild (item.Id);
      }
    }

    TContentItemModel Select (Guid contentId)
    {
      foreach (var item in m_ContentItems) {
        if (item.ContainsId (contentId)) {
          return (item);
        }
      }

      return (null);
    }

    TContentItemModel Select (TPosition position)
    {
      foreach (var item in m_ContentItems) {
        if (item.IsPosition (position)) {
          return (item);
        }
      }

      return (null);
    }

    TContentItemModel Select (int col, int row)
    {
      foreach (var item in m_ContentItems) {
        if (item.IsPosition (TPosition.Create (col, row))) {
          return (item);
        }
      }

      return (null);
    }

    int SelectByIndex (Guid contentId)
    {
      for (int index = 0; index < m_ContentItems.Count; index++) {
        var item = m_ContentItems [index];

        if (item.ContainsId (contentId)) {
          return (index);
        }
      }

      return (-1);
    }

    void ChangePosition (TPosition position, TContentItemModel item)
    {
      if (item.NotNull ()) {
        item.ChangePosition (position);
      }
    }

    void UpdateLayout (TPosition position)
    {
      UpdateLayout (position.Column , position.Row);
    }

    void UpdateLayout (int col, int row)
    {
      // rows 
      if (SizeRows.Equals (3)) {
        if (row.Equals (2)) {
          // Fill row 1
          m_ContentContainer.RowDefinitions [0].Height = new GridLength (116, GridUnitType.Pixel);
        }

        if (row.Equals (3)) {
          // Fill row 1, 2
          m_ContentContainer.RowDefinitions [0].Height = new GridLength (116, GridUnitType.Pixel);
          m_ContentContainer.RowDefinitions [1].Height = new GridLength (116, GridUnitType.Pixel);
        }
      }

      // cols
      if (SizeCols.Equals (4)) {
        if (col.Equals (2)) {
          // Fill col 1
          m_ContentContainer.ColumnDefinitions [0].Width = new GridLength (300, GridUnitType.Pixel);
        }

        if (col.Equals (3)) {
          // Fill col 1,2
          m_ContentContainer.ColumnDefinitions [0].Width = new GridLength (300, GridUnitType.Pixel);
          m_ContentContainer.ColumnDefinitions [1].Width = new GridLength (300, GridUnitType.Pixel);
        }

        if (col.Equals (4)) {
          // Fill col 1,2,3
          m_ContentContainer.ColumnDefinitions [0].Width = new GridLength (300, GridUnitType.Pixel);
          m_ContentContainer.ColumnDefinitions [1].Width = new GridLength (300, GridUnitType.Pixel);
          m_ContentContainer.ColumnDefinitions [2].Width = new GridLength (300, GridUnitType.Pixel);
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace