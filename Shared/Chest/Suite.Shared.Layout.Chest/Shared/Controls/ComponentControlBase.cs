/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Layout.Chest
{
  public abstract class TComponentControlBase : ListBox
  {
    #region Dependency Property
    public static readonly DependencyProperty ModelProperty =
      DependencyProperty.Register ("Model", typeof (TComponentControlModel), typeof (TComponentControlBase),
      new FrameworkPropertyMetadata (null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ModelPropertyChanged));
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

      HorizontalAlignment = HorizontalAlignment.Stretch;
      VerticalAlignment = VerticalAlignment.Stretch;
      BorderThickness = new Thickness (0);
      
      ModelValidated = false;
    }
    #endregion

    #region Members
    public void InsertContent (IList<TComponentModelItem> contentCollection)
    {
      // contentCollection contains only Drawer
      if (contentCollection.NotNull ()) {
        Cleanup ();

        foreach (var content in contentCollection) {
          var control = CreateControl (content);

          if (control.NotNull ()) {
            AddToCollection (control);
          }
        }

        RefreshCollection ();
      }
    }

    public void InsertContent (TComponentModelItem item)
    {
      // contains only Drawer
      if (item.NotNull ()) {
        var control = CreateControl (item);

        if (control.NotNull ()) {
          AddToCollection (control);
        }

        RefreshCollection ();
      }
    }

    public void RemoveContent (Guid id)
    {
      if (id.NotEmpty ()) {
        var list = m_ItemsSource
          .Where (p => p.Id.Equals (id))
          .ToList ()
        ;

        if (list.Count.Equals (1)) {
          var control = list [0];
          m_ItemsSource.Remove (control);
        }

        RefreshCollection ();
      }
    }

    public void MoveContent (Guid id, int position)
    {
      if (id.NotEmpty ()) {
        if ((position > -1) && (position < m_ItemsSource.Count)) {
          for (int index = 0; index < m_ItemsSource.Count; index++) {
            var itemId = m_ItemsSource [index].Id;

            if (id.Equals (itemId)) {
              m_ItemsSource.Move (index, position);
              break;
            }  
          }
        }

        RefreshCollection ();
      }
    }

    public void Cleanup ()
    {
      ItemsSource = null;

      m_ItemsSource = new ObservableCollection<Drawer.TComponentDisplayControl> ();

      m_CollectionViewSource = new CollectionViewSource
      {
        Source = m_ItemsSource
      };

      ItemsSource = m_CollectionViewSource.View;
    }
    #endregion

    #region Callback
    static void ModelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TComponentControlBase control) {
        if (e.NewValue is TComponentControlModel) {
          control.ModelValidated = true; // first time
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
    #endregion

    #region Fields
    CollectionViewSource                                                                      m_CollectionViewSource;
    ObservableCollection<Shared.Layout.Drawer.TComponentDisplayControl>                       m_ItemsSource;
    #endregion

    #region Support
    Shared.Layout.Drawer.TComponentDisplayControl CreateControl (TComponentModelItem modelItem)
    {
      if (modelItem.NotNull ()) {
        var controlModel = Shared.Layout.Drawer.TComponentControlModel.CreateDefault;
        controlModel.Select (modelItem);

        var displayControl = new Shared.Layout.Drawer.TComponentDisplayControl ()
        {
          Model = controlModel
        };

        displayControl.ChangeSize (modelItem.Size);
        displayControl.InsertContent (modelItem.ChildCollection);

        return (displayControl);
      }

      return (null);
    }

    void AddToCollection (Shared.Layout.Drawer.TComponentDisplayControl control)
    {
      m_ItemsSource.Add (control);
    }

    void RefreshCollection ()
    {
      m_CollectionViewSource.View.Refresh ();
    }
    #endregion
  };
  //---------------------------//

}  // namespace