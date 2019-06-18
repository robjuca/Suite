/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Windows.Controls;
//---------------------------//

namespace Shared.Gadget.Image
{
  public sealed  class TComponentControl : ListBox
  {
    #region Dependency Property
    public static readonly DependencyProperty ModelProperty =
      DependencyProperty.Register ("Model", typeof (TComponentControlModel), typeof (TComponentControl),
      new FrameworkPropertyMetadata (TComponentControlModel.CreateDefault, ModelPropertyChanged));
    #endregion

    #region Property
    public TComponentControlModel Model
    {
      get
      {
        return ((TComponentControlModel) GetValue (ModelProperty));
      }

      set
      {
        SetValue (ModelProperty, value);
      }
    }

    public Guid Id
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TComponentControl ()
    {
      Margin = new Thickness (0);
      Padding = new Thickness (0);
      BorderThickness = new Thickness (0);

      HorizontalAlignment = HorizontalAlignment.Stretch;
      VerticalAlignment = VerticalAlignment.Stretch;

      SetValue (ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
      SetValue (ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);

      string normalTemplate = @"
        <DataTemplate
            xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
            xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
            <Image Stretch='None' Source='{Binding}'/>
        </DataTemplate>"
      ;

      using (var sr = new System.IO.MemoryStream (System.Text.Encoding.UTF8.GetBytes (normalTemplate))) {
        ItemTemplate = System.Windows.Markup.XamlReader.Load (sr) as DataTemplate;
      }
    }
    #endregion

    #region Members
    public void RefreshDesign ()
    {
      ItemsSource = null;

      if (Model.NotNull ()) {
        ItemsSource = Model.Frames;
      }
    }

    public void Cleanup ()
    {
      ItemsSource = null;
    }
    #endregion

    #region Callback
    static void ModelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TComponentControl control) {
        if (e.NewValue is TComponentControlModel model) {
          // do nothing
        }
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace