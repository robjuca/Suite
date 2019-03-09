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
//---------------------------//

namespace Shared.Layout.Drawer
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
        return (Model.Id);
      }
    }
    #endregion

    #region Constructor
    public TComponentControlBase ()
    {
      MyType = TType.None;

      HorizontalAlignment = HorizontalAlignment.Left;
      VerticalAlignment = VerticalAlignment.Top;

      ModelValidated = false;

      m_ContentItems = new Collection<TContentItemModel> ();
    }
    #endregion

    #region Members
    public void ChangeSize (TSize size)
    {
      SizeCols = size.Columns;
      SizeRows = size.Rows;
    }

    public void InsertContent (Server.Models.Component.TEntityAction action)
    {
      // action.ModelAction {shelf model}
      // action.CollectionAction.EntityCollection {id, model}
      // action.CollectionAction.ComponentOperation

      if (action.NotNull ()) {
        var componentModel = Server.Models.Component.TComponentModel.Create (action.ModelAction);

        var componentModelItem = TComponentModelItem.Create (componentModel);
        componentModelItem.Select (action.CategoryType.Category); // Self

        // Shelf child {Bag}
        foreach (var item in action.CollectionAction.EntityCollection) {
          var childId = item.Key;
          var childAction = item.Value;
          var childComponentModel = Server.Models.Component.TComponentModel.Create (childAction.ModelAction); // Bag model

          if (action.CollectionAction.ComponentOperation.ParentIdCollection.ContainsKey (componentModel.Id)) {
            var componentOperationList = action.CollectionAction.ComponentOperation.ParentIdCollection [componentModel.Id];

            foreach (var componentRelation in componentOperationList) {
              if (componentRelation.ChildId.Equals (childId)) {
                childComponentModel.GeometryModel.PositionCol = componentRelation.PositionColumn;
                childComponentModel.GeometryModel.PositionRow = componentRelation.PositionRow;
              }
            }
          }

          var childComponentModelItem = TComponentModelItem.Create (childComponentModel);
          childComponentModelItem.Select (Server.Models.Infrastructure.TCategory.Bag);

          foreach (var node in childAction.CollectionAction.ExtensionNodeCollection) {
            childComponentModelItem.NodeModelCollection.Add (node);

            if (childAction.CollectionAction.ModelCollection.ContainsKey (node.ChildId)) {
              var childNodeModel = childAction.CollectionAction.ModelCollection [node.ChildId];
              var childNodeComponentModel = Server.Models.Component.TComponentModel.Create (childNodeModel); // Bag child node {DOC, IMG, VIDEO}

              var childNodeComponentModelItem = TComponentModelItem.Create (childNodeComponentModel);
              childNodeComponentModelItem.Select (Server.Models.Infrastructure.TCategoryType.FromValue (node.ChildCategory));

              childComponentModelItem.ChildCollection.Add (childNodeComponentModelItem);
            }
          }

          componentModelItem.ChildCollection.Add (childComponentModelItem);
        }

        var list = new List<TComponentModelItem>
        {
          componentModelItem
        };

        InsertContent (list);
      }
    }

    public void InsertContent (IList<TComponentModelItem> contentCollection)
    {
      // contentCollection contains only Shelf
      if (contentCollection.NotNull ()) {
        foreach (var content in contentCollection) {
          InsertChild (content);
        }
      }
    }

    //public void InsertContent (TPosition position, Shared.Module.Shelf.TComponentControlModel model)
    //{
    //  m_ContentItems.Add (new TContentItemModel (position, model));

    //  InsertChild (position, model);

    //  if (MyType.Equals (TType.Display)) {
    //    UpdateLayout (position);
    //  }
    //}

    //public void ChangeContent (Guid id, TComponentModelItem item)
    //{
    //  int index = SelectByIndex (bagId);

    //  if (index > -1) {
    //    m_ContentItems [index].BagItem.CopyFrom (bagItem);

    //    for (index = 0; index < m_ContentContainer.Children.Count; index++) {
    //      var border = m_ContentContainer.Children [index] as Border;

    //      if (border.Child is Bag.TDisplayControl child) {
    //        if (child.Id.Equals (bagId)) {
    //          child.Model = CreateModel (bagItem);
    //          child.RefreshDesign ();
    //          break;
    //        }
    //      }
    //    }
    //  }
    //}

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

      UpdateLayout (source);
      UpdateLayout (target);
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
          control.ModelValidated = true; // first time
          control.RefreshDesign ();
        }
      }
    }
    #endregion

    #region Data
    public enum TType
    {
      None,
      Design,
      Display,
    };
    #endregion

    #region Property
    public TType MyType
    {
      get;
      set;
    }

    public bool ModelValidated
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
    public Grid                                                 m_ContentContainer;
    public Collection<TContentItemModel>                        m_ContentItems;
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
    void InsertChild (TComponentModelItem modelItem)
    {
      if (modelItem.NotNull ()) {
        var position = modelItem.Position;
        var size = modelItem.Size;

        var controlModel = Shared.Layout.Shelf.TComponentControlModel.CreateDefault;
        controlModel.Select (modelItem);

        m_ContentItems.Add (new TContentItemModel (position, controlModel));

        var displayControl = new Shared.Layout.Shelf.TComponentDisplayControl (controlModel);

        var border = new Border ()
        {
          Background = Brushes.White,
          Child = displayControl,
        };

        RequestBorder (border);
        RequestRoom (position, size, border);

        m_ContentContainer.Children.Add (border);

        displayControl.InsertContent (modelItem.ChildCollection);
      }
    }

    void InsertChild (TContentItemModel item)
    {
      if (item.NotNull ()) {
        var position = item.Position;
        var size = item.ComponentControlModel.Size;
        var controlModel = item.ComponentControlModel;

        m_ContentItems.Add (new TContentItemModel (position, controlModel));

        var displayControl = new Shared.Layout.Shelf.TComponentDisplayControl (controlModel);

        var border = new Border ()
        {
          Background = Brushes.White,
          Child = displayControl,
        };

        RequestBorder (border);
        RequestRoom (position, size, border);

        m_ContentContainer.Children.Add (border);

        displayControl.InsertContent (controlModel.ComponentModelItem.ChildCollection);
      }
    }

    //void InsertChild (TPosition position, Shared.Module.Shelf.TComponentControlModel model)
    //{
    //  var displayControl = new Shared.Module.Shelf.TComponentDisplayControl (model);

    //  var border = new Border ()
    //  {
    //    Background = Brushes.White,
    //    Child = displayControl,
    //  };

    //  RequestBorder (border);

    //  var col = position.Column;
    //  var row = position.Row;


    //  //switch (model.Style) {
    //  //  case "mini":
    //  //    border.SetValue (Grid.ColumnProperty, (col - 1));
    //  //    border.SetValue (Grid.RowProperty, (row - 1));
    //  //    break;

    //  //  case "small":
    //  //    border.SetValue (Grid.ColumnProperty, (col - 1));
    //  //    border.SetValue (Grid.RowProperty, (row - 1));
    //  //    border.SetValue (Grid.RowSpanProperty, 2);
    //  //    break;

    //  //  case "large":
    //  //    border.SetValue (Grid.ColumnProperty, (col - 1));
    //  //    border.SetValue (Grid.RowProperty, (row - 1));
    //  //    border.SetValue (Grid.RowSpanProperty, 3);
    //  //    break;

    //  //  case "big":
    //  //    border.SetValue (Grid.ColumnProperty, (col - 1));
    //  //    border.SetValue (Grid.ColumnSpanProperty, 2);
    //  //    border.SetValue (Grid.RowSpanProperty, 3);
    //  //    break;
    //  //}

    //  m_ContentContainer.Children.Add (border);

    //  //displayControl.Refresh ();
    //}



    void RemoveChild (Guid contentId)
    {
      if (contentId.NotEmpty ()) {
        for (int index = 0; index < m_ContentContainer.Children.Count; index++) {
          var border = m_ContentContainer.Children [index] as Border;

          if (border.Child is Shared.Layout.Shelf.TComponentDisplayControl bagChild) {
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

    void UpdateLayout (TContentItemModel item)
    {
      if (item.NotNull ()) {
        UpdateLayout (item.Position);
      }
    }

    void UpdateLayout (TPosition position)
    {
      UpdateLayout (position.Column, position.Row);
    }

    void UpdateLayout (int col, int row)
    {
      // rows 
      if (SizeRows.Equals (3)) {
        if (row.Equals (2)) {
          // Fill row 1, 3
          m_ContentContainer.RowDefinitions [0].Height = new GridLength (116, GridUnitType.Pixel);
          m_ContentContainer.RowDefinitions [2].Height = new GridLength (116, GridUnitType.Pixel);
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

    void RequestRoom (TPosition position, TSize size, Border border)
    {
      var col = position.Column;
      var row = position.Row;
      var colSize = size.Columns;
      var rowSize = size.Rows;

      if ((((col - 1) + colSize) <= SizeCols) && (((row - 1) + rowSize) <= SizeRows)) {
        border.SetValue (Grid.ColumnProperty, (col - 1));
        border.SetValue (Grid.RowProperty, (row - 1));

        border.SetValue (Grid.ColumnSpanProperty, colSize);
        border.SetValue (Grid.RowSpanProperty, rowSize);
      }

      UpdateLayout (position);
    }
    #endregion
  };
  //---------------------------//

}  // namespace