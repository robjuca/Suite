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

    public string StyleHorizontalName
    {
      get
      {
        return (string) GetValue (StyleHorizontalNameProperty);
      }

      private set
      {
        SetValue (StyleHorizontalNameProperty, value);
      }
    }

    public string StyleVerticalName
    {
      get
      {
        return (string) GetValue (StyleVerticalNameProperty);
      }

      private set
      {
        SetValue (StyleVerticalNameProperty, value);
      }
    }

    public string SelectStyleHorizontal
    {
      get
      {
        return (string) GetValue (SelectStyleHorizontalProperty);
      }

      set
      {
        SetValue (SelectStyleHorizontalProperty, value);
      }
    }

    public string SelectStyleVertical
    {
      get
      {
        return (string) GetValue (SelectStyleVerticalProperty);
      }

      set
      {
        SetValue (SelectStyleVerticalProperty, value);
      }
    }
    #endregion

    #region Dependency Property
    public static readonly DependencyProperty LayoutProperty =
        DependencyProperty.Register ("Layout", typeof (string), typeof (TStyleSelector),
        new PropertyMetadata (string.Empty));

    public static readonly DependencyProperty StyleHorizontalNameProperty =
        DependencyProperty.Register ("StyleHorizontalName", typeof (string), typeof (TStyleSelector), 
        new PropertyMetadata (string.Empty));

    public static readonly DependencyProperty StyleVerticalNameProperty =
        DependencyProperty.Register ("StyleVerticalName", typeof (string), typeof (TStyleSelector),
        new PropertyMetadata (string.Empty));

    public static readonly DependencyProperty SelectStyleHorizontalProperty =
        DependencyProperty.Register ("SelectStyleHorizontal", typeof (string), typeof (TStyleSelector), 
        new FrameworkPropertyMetadata (string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectStyleHorizontalPropertyChanged));

    public static readonly DependencyProperty SelectStyleVerticalProperty =
        DependencyProperty.Register ("SelectStyleVertical", typeof (string), typeof (TStyleSelector),
        new FrameworkPropertyMetadata (string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectStyleVerticalPropertyChanged));
    #endregion

    #region Event
    // Declare the delegate (if using non-generic pattern).
    public delegate void StyleHorizontalSelectedEventHandler (object sender, EventArgs e);
    public delegate void StyleVerticalSelectedEventHandler (object sender, EventArgs e);

    // Declare the event.
    public event StyleHorizontalSelectedEventHandler StyleHorizontalSelected;
    public event StyleVerticalSelectedEventHandler StyleVerticalSelected;
    #endregion

    #region Constructor
    public TStyleSelector ()
    {
      Layout = "horizontal"; // can be: 'horizontal' or 'vertical'

      // radio buttons horizontal
      m_PartsHorizontal = new Dictionary<string, RadioButton> ();

      for (int i = 0; i < m_Styles.Length; i++) {
        var rb = new RadioButton
        {
          Margin = new Thickness (2),
          HorizontalAlignment=HorizontalAlignment.Center,
          VerticalAlignment = VerticalAlignment.Center,
          GroupName = "StyleHorizontalSelector",
          Tag = m_Styles [i]
        };

        rb.Checked += OnStyleHorizontalChecked;

        m_PartsHorizontal.Add (m_Styles [i], rb);
      }

      // radio buttons vertical
      m_PartsVertical = new Dictionary<string, RadioButton> ();

      for (int i = 0; i < m_Styles.Length; i++) {
        var rb = new RadioButton
        {
          Margin = new Thickness (2),
          HorizontalAlignment = HorizontalAlignment.Center,
          VerticalAlignment = VerticalAlignment.Center,
          GroupName = "StyleVerticalSelector",
          Tag = m_Styles [i]
        };

        rb.Checked += OnStyleVerticalChecked;

        m_PartsVertical.Add (m_Styles [i], rb);
      }

      DefaultStyleKey = typeof (TStyleSelector);
    }
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      base.OnApplyTemplate ();

      // horizontal
      var layoutH = new string [] { PART_HORIZONTAL_C0, PART_HORIZONTAL_C1, PART_HORIZONTAL_C2, PART_HORIZONTAL_C3 };

      for (int i = 0; i < layoutH.Length; i++) {
        if (GetTemplateChild (layoutH [i]) is Border border) {
          border.Child = m_PartsHorizontal [m_Styles [i]];
        }
      }

      // vertical
      var layoutV = new string [] { PART_VERTICAL_R0, PART_VERTICAL_R1, PART_VERTICAL_R2, PART_VERTICAL_R3 };

      for (int i = 0; i < layoutV.Length; i++) {
        if (GetTemplateChild (layoutV [i]) is Border border) {
          border.Child = m_PartsVertical [m_Styles [i]];
        }
      }
    }
    #endregion

    #region Event
    void OnStyleHorizontalChecked (object sender, RoutedEventArgs e)
    {
      if (sender is RadioButton rb) {
        StyleHorizontalName = rb.Tag.ToString ();

        StyleHorizontalSelected?.Invoke (this, EventArgs.Empty);
      }
    }

    void OnStyleVerticalChecked (object sender, RoutedEventArgs e)
    {
      if (sender is RadioButton rb) {
        StyleVerticalName = rb.Tag.ToString ();

        StyleVerticalSelected?.Invoke (this, EventArgs.Empty);
      }
    }

    static void SelectStyleHorizontalPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TStyleSelector selector) {
        if (e.NewValue is string style) {
          selector.TryToSelect (TResource.TStyleMode.Horizontal, style);
        }
      }
    }

    static void SelectStyleVerticalPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TStyleSelector selector) {
        if (e.NewValue is string style) {
          selector.TryToSelect (TResource.TStyleMode.Vertical, style);
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
    readonly Dictionary<string, RadioButton>                              m_PartsHorizontal;
    readonly Dictionary<string, RadioButton>                              m_PartsVertical;
    readonly string []                                                    m_Styles = new string [] { MINI, SMALL, LARGE, BIG };
    #endregion

    #region Support
    void TryToSelect (TResource.TStyleMode mode, string style)
    {
      switch (mode) {
        case TResource.TStyleMode.Horizontal:
          if (m_PartsHorizontal.ContainsKey (style)) {
            m_PartsHorizontal [style].IsChecked = true;
          }
          break;

        case TResource.TStyleMode.Vertical:
          if (m_PartsVertical.ContainsKey (style)) {
            m_PartsVertical [style].IsChecked = true;
          }
          break;
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace