/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
//---------------------------//

namespace Shared.Resources
{
  [TemplatePart (Name = PART_HORIZONTAL_LAYOUT, Type = typeof (Border))]
  [TemplatePart (Name = PART_VERTICAL_LAYOUT, Type = typeof (Border))]

  [TemplatePart (Name = PART_HORIZONTAL_C0, Type = typeof (Border))]
  [TemplatePart (Name = PART_HORIZONTAL_C1, Type = typeof (Border))]
  [TemplatePart (Name = PART_HORIZONTAL_C2, Type = typeof (Border))]
  [TemplatePart (Name = PART_HORIZONTAL_C3, Type = typeof (Border))]

  [TemplatePart (Name = PART_VERTICAL_R0, Type = typeof (Border))]
  [TemplatePart (Name = PART_VERTICAL_R1, Type = typeof (Border))]
  [TemplatePart (Name = PART_VERTICAL_R2, Type = typeof (Border))]
  [TemplatePart (Name = PART_VERTICAL_R3, Type = typeof (Border))]
  public sealed class TStyleSelector : Control
  {
    #region Property
    public string Layout
    {
      get
      {
        return (string) GetValue (LayoutProperty);
      }

      set
      {
        SetValue (LayoutProperty, value);
      }
    }

    public string StyleName
    {
      get
      {
        return (string) GetValue (StyleNameProperty);
      }

      private set
      {
        SetValue (StyleNameProperty, value);
      }
    }

    public string SelectStyle
    {
      get
      {
        return (string) GetValue (SelectStyleProperty);
      }

      set
      {
        SetValue (SelectStyleProperty, value);
      }
    }
    #endregion

    #region Dependency Property
    public static readonly DependencyProperty LayoutProperty =
        DependencyProperty.Register ("Layout", typeof (string), typeof (TStyleSelector),
        new PropertyMetadata (string.Empty));

    public static readonly DependencyProperty StyleNameProperty =
        DependencyProperty.Register ("StyleName", typeof (string), typeof (TStyleSelector), 
        new PropertyMetadata (string.Empty));

    public static readonly DependencyProperty SelectStyleProperty =
        DependencyProperty.Register ("SelectStyle", typeof (string), typeof (TStyleSelector), 
        new FrameworkPropertyMetadata (string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectStylePropertyChanged));
    #endregion

    #region Event
    // Declare the delegate (if using non-generic pattern).
    public delegate void StyleSelectedEventHandler (object sender, EventArgs e);

    // Declare the event.
    public event StyleSelectedEventHandler StyleSelected; 
    #endregion

    #region Constructor
    public TStyleSelector ()
    {
      Layout = "horizontal"; // can be: 'horizontal' or 'vertical'

      // radio buttons
      m_Parts = new Dictionary<string, RadioButton> ();
      string [] styles = new string [] { MINI, SMALL, LARGE, BIG };

      for (int i = 0; i < styles.Length; i++) {
        var rb = new RadioButton
        {
          Margin = new Thickness (2),
          VerticalAlignment = VerticalAlignment.Center,
          Content = styles [i],
          GroupName = "StyleSelector"
        };

        rb.Checked += OnStyleChecked;

        m_Parts.Add (styles [i], rb);
      }

      DefaultStyleKey = typeof (TStyleSelector);
    }
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      base.OnApplyTemplate ();

      string [] layout = new string [4];

      if (Layout.Equals ("horizontal")) {
         layout = new string [] {PART_HORIZONTAL_C0, PART_HORIZONTAL_C1, PART_HORIZONTAL_C2, PART_HORIZONTAL_C3 };
      }

      if (Layout.Equals ("vertical")) {
        layout = new string [] { PART_VERTICAL_R0, PART_VERTICAL_R1, PART_VERTICAL_R2, PART_VERTICAL_R3 };
      }

      string [] styles = new string [] { MINI, SMALL, LARGE, BIG };

      for (int i = 0; i < layout.Length; i++) {
        if (GetTemplateChild (layout [i]) is Border border) {
          border.Child = m_Parts [styles [i]];
        }
      }
    }
    #endregion

    #region Event
    void OnStyleChecked (object sender, RoutedEventArgs e)
    {
      if (sender is RadioButton rb) {
        StyleName = rb.Content.ToString ();

        StyleSelected?.Invoke (this, EventArgs.Empty);
      }
    }

    static void SelectStylePropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TStyleSelector selector) {
        if (e.NewValue is string style) {
          selector.TryToSelect (style);
        }
      }
    }
    #endregion

    #region Constants
    const string PART_HORIZONTAL_LAYOUT               = "PART_HorizontalLayout";
    const string PART_VERTICAL_LAYOUT                 = "PART_VerticalLayout";

    const string PART_HORIZONTAL_C0                   = "PART_HorizontalC0";
    const string PART_HORIZONTAL_C1                   = "PART_HorizontalC1";
    const string PART_HORIZONTAL_C2                   = "PART_HorizontalC2";
    const string PART_HORIZONTAL_C3                   = "PART_HorizontalC3";

    const string PART_VERTICAL_R0                     = "PART_VerticalR0";
    const string PART_VERTICAL_R1                     = "PART_VerticalR1";
    const string PART_VERTICAL_R2                     = "PART_VerticalR2";
    const string PART_VERTICAL_R3                     = "PART_VerticalR3";

    const string MINI                                 = "mini";
    const string SMALL                                = "small";
    const string LARGE                                = "large";
    const string BIG                                  = "big";
    #endregion

    #region Fields
    Dictionary<string, RadioButton>                       m_Parts;
    #endregion

    #region Support
    void TryToSelect (string style)
    {
      if (m_Parts.ContainsKey (style)) {
        m_Parts [style].IsChecked = true;
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace