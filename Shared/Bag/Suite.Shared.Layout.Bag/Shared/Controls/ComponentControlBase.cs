/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Windows.Controls;

using Shared.Types;
//---------------------------//

namespace Shared.Layout.Bag
{
  public abstract class TComponentControlBase : Border
  {
    #region Property
    public TComponentControlModel Model
    {
      get
      {
        return (TComponentControlModel) GetValue (ComponentControlModelProperty);
      }
      set
      {
        SetValue (ComponentControlModelProperty, value);
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

    #region Dependency Property
    public static readonly DependencyProperty ComponentControlModelProperty =
      DependencyProperty.Register ("Model", typeof (TComponentControlModel), typeof (TComponentControlBase),
      new FrameworkPropertyMetadata (TComponentControlModel.CreateDefault, ModelPropertyChanged));
    #endregion

    #region Constructor
    TComponentControlBase ()
    {
      m_DocumentControl = new Shared.Gadget.Document.TComponentDisplayControl
      {
        Visibility = Visibility.Collapsed,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
      };

      m_ImageControl = new Shared.Gadget.Image.TComponentControl
      {
        Visibility = Visibility.Collapsed,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
      };

      var grid = new Grid ();
      grid.Children.Add (m_DocumentControl);
      grid.Children.Add (m_ImageControl);

      Child = grid;

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
    public void Select (Server.Models.Infrastructure.TCategory category, Shared.Gadget.Document.TComponentControlModel model)
    {
      if (category.Equals (Server.Models.Infrastructure.TCategory.Document)) {
        m_DocumentControl.Model.Cleanup ();
        m_DocumentControl.Model.CopyFrom (model);
        m_DocumentControl.Model.PropertyName = "all";
        m_DocumentControl.Visibility = Visibility.Visible;
        m_DocumentControl.RefreshDesign ();
      }
    }

    public void Select (Server.Models.Infrastructure.TCategory category, Shared.Gadget.Image.TComponentControlModel model)
    {
      if (category.Equals (Server.Models.Infrastructure.TCategory.Image)) {
        m_ImageControl.Model.Cleanup ();
        m_ImageControl.Model.CopyFrom (model);
        m_ImageControl.Visibility = Visibility.Visible;
        m_ImageControl.RefreshDesign ();
      }
    }

    public void SelectModel ()
    {
      m_DocumentControl.Visibility = Visibility.Collapsed;
      m_ImageControl.Visibility = Visibility.Collapsed;

      var category = ControlModelMode.Equals (TControlModelMode.Default) ? Model.ChildCategory : ControlModelMode.Equals (TControlModelMode.Local) ? ModelLocal.ChildCategory : Server.Models.Infrastructure.TCategory.None;

      switch (ControlModelMode) {
        case TControlModelMode.Local:
          Select (category, ModelLocal.ComponentDocumentControlModel);
          Select (category, ModelLocal.ComponentImageControlModel);
          break;

        case TControlModelMode.Default:
          Select (category, Model.ComponentDocumentControlModel);
          Select (category, Model.ComponentImageControlModel);
          break;
      }
    }

    public void Refresh ()
    {
      SelectModel ();
    }

    public void Cleanup ()
    {
      m_DocumentControl.Cleanup ();
      m_ImageControl.Cleanup ();
    }
    #endregion

    #region Callback
    static void ModelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TComponentControlBase control) {
        if (e.NewValue is TComponentControlModel) {
          control.SelectModel ();
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
    #endregion

    #region Fields
    readonly Shared.Gadget.Document.TComponentDisplayControl              m_DocumentControl;
    readonly Shared.Gadget.Image.TComponentControl                        m_ImageControl; 
    #endregion
  };
  //---------------------------//

}  // namespace