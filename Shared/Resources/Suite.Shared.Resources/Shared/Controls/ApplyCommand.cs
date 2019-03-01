/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Windows.Controls;
//---------------------------//

namespace Shared.Resources
{
  [TemplatePart (Name = PART_PANEL, Type = typeof (Border))]
  [TemplatePart (Name = PART_PROGRESSBAR, Type = typeof (ProgressBar))]
  [TemplatePart (Name = PART_MESSAGETEXT, Type = typeof (TextBlock))]
  [TemplatePart (Name = PART_APPLYBUTTON, Type = typeof (Button))]
  [TemplatePart (Name = PART_CANCELBUTTON, Type = typeof (Button))]
  [TemplatePart (Name = PART_CONTENTBUSY, Type = typeof (ContentControl))]
  public sealed class TApplyCommand : Control
  {
    #region Property
    public string ShowPanel
    {
      get
      {
        return (string) GetValue (ShowPanelProperty);
      }

      set
      {
        SetValue (ShowPanelProperty, value);
      }
    }

    public string ProgressBarActive
    {
      get
      {
        return (string) GetValue (ProgressBarActiveProperty);
      }

      set
      {
        SetValue (ProgressBarActiveProperty, value);
      }
    }

    public string MessageText
    {
      get
      {
        return (string) GetValue (MessageTextProperty);
      }

      set
      {
        SetValue (MessageTextProperty, value);
      }
    }

    public string ApplyEnabled
    {
      get
      {
        return (string) GetValue (ApplyEnabledProperty);
      }

      set
      {
        SetValue (ApplyEnabledProperty, value);
      }
    }

    public string CancelEnabled
    {
      get
      {
        return (string) GetValue (CancelEnabledProperty);
      }

      set
      {
        SetValue (CancelEnabledProperty, value);
      }
    }

    public string ContentBusyVisibility
    {
      get
      {
        return (string) GetValue (ContentBusyVisibilityProperty);
      }

      set
      {
        SetValue (ContentBusyVisibilityProperty, value);
      }
    }
    #endregion

    #region Dependency Property
    public static readonly DependencyProperty ShowPanelProperty =
        DependencyProperty.Register ("ShowPanel", typeof (bool), typeof (TApplyCommand),
        new FrameworkPropertyMetadata (false, ShowPanelPropertyChanged));

    public static readonly DependencyProperty ProgressBarActiveProperty =
        DependencyProperty.Register ("ProgressBarActive", typeof (bool), typeof (TApplyCommand),
        new FrameworkPropertyMetadata (false, ProgressBarActivePropertyChanged));

    public static readonly DependencyProperty MessageTextProperty =
        DependencyProperty.Register ("MessageText", typeof (string), typeof (TApplyCommand),
        new FrameworkPropertyMetadata (string.Empty, MessageTextPropertyChanged));

    public static readonly DependencyProperty ApplyEnabledProperty =
        DependencyProperty.Register ("ApplyEnabled", typeof (bool), typeof (TApplyCommand),
        new FrameworkPropertyMetadata (false, ApplyEnabledPropertyChanged));

    public static readonly DependencyProperty CancelEnabledProperty =
        DependencyProperty.Register ("CancelEnabled", typeof (bool), typeof (TApplyCommand),
        new FrameworkPropertyMetadata (false, CancelEnabledPropertyChanged));

    public static readonly DependencyProperty ContentBusyVisibilityProperty =
        DependencyProperty.Register ("ContentBusyVisibility", typeof (Visibility), typeof (TApplyCommand),
        new FrameworkPropertyMetadata (Visibility.Collapsed, ContentBusyVisibilityPropertyChanged));
    #endregion

    #region Event
    // Declare the delegate (if using non-generic pattern).
    public delegate void ApplyCommandEventHandler (object sender, EventArgs e);
    public delegate void CancelCommandEventHandler (object sender, EventArgs e);

    // Declare the event.
    public event ApplyCommandEventHandler Apply;
    public event CancelCommandEventHandler Cancel;
    #endregion

    #region Constructor
    public TApplyCommand ()
    {
      m_ApplyEnabled = false;
      m_CancelEnabled = false;
      m_ContentBusy = Visibility.Collapsed;
    }

    static TApplyCommand ()
    {
      DefaultStyleKeyProperty.OverrideMetadata (typeof (TApplyCommand), new FrameworkPropertyMetadata (typeof (TApplyCommand)));
    }
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      base.OnApplyTemplate ();

      if (GetTemplateChild (PART_PANEL) is Border panel) {
        panel.Visibility = Visibility.Collapsed;
      }

      if (GetTemplateChild (PART_PROGRESSBAR) is ProgressBar bar) {
        bar.IsIndeterminate = false;
      }

      if (GetTemplateChild (PART_MESSAGETEXT) is TextBlock message) {
        message.Text = string.Empty;
      }

      if (GetTemplateChild (PART_APPLYBUTTON) is Button butApply) {
        butApply.IsEnabled = m_ApplyEnabled;
        butApply.Click += OnApplyClicked;
      }

      if (GetTemplateChild (PART_CANCELBUTTON) is Button butCancel) {
        butCancel.IsEnabled = m_CancelEnabled;
        butCancel.Click += OnCancelClicked;
      }

      if (GetTemplateChild (PART_CONTENTBUSY) is ContentControl content) {
        content.Visibility = m_ContentBusy;
      }
    }
    #endregion

    #region Event
    void OnApplyClicked (object sender, RoutedEventArgs e)
    {
      if (sender is Button button) {
        Apply?.Invoke (this, EventArgs.Empty);
      }
    }

    void OnCancelClicked (object sender, RoutedEventArgs e)
    {
      if (sender is Button button) {
        Cancel?.Invoke (this, EventArgs.Empty);
      }
    }
    #endregion

    #region Callback
    static void ShowPanelPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TApplyCommand control) {
        if (e.NewValue is bool show) {
          if (control.GetTemplateChild (PART_PANEL) is Border panel) {
            panel.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
          }
        }
      }
    }

    static void ProgressBarActivePropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TApplyCommand control) {
        if (e.NewValue is bool active) {
          if (control.GetTemplateChild (PART_PROGRESSBAR) is ProgressBar bar) {
            bar.IsIndeterminate = active;
          }
        }
      }
    }

    static void MessageTextPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TApplyCommand control) {
        if (e.NewValue is string message) {
          if (control.GetTemplateChild (PART_MESSAGETEXT) is TextBox text) {
            text.Text = message;
          }
        }
      }
    }

    static void ApplyEnabledPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TApplyCommand control) {
        if (e.NewValue is bool enabled) {
          if (control.GetTemplateChild (PART_APPLYBUTTON) is Button button) {
            button.IsEnabled = enabled;
          }

          else {
            control.m_ApplyEnabled = enabled;
          }
        }
      }
    }

    static void CancelEnabledPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TApplyCommand control) {
        if (e.NewValue is bool enabled) {
          if (control.GetTemplateChild (PART_CANCELBUTTON) is Button button) {
            button.IsEnabled = enabled;
          }

          else {
            control.m_CancelEnabled = enabled;
          }
        }
      }
    }

    static void ContentBusyVisibilityPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TApplyCommand control) {
        if (e.NewValue is Visibility visibility) {
          if (control.GetTemplateChild (PART_CONTENTBUSY) is ContentControl content) {
            content.Visibility = visibility;
          }

          else {
            control.m_ContentBusy = visibility;
          }
        }
      }
    }
    #endregion

    #region Fields
    Visibility                              m_ContentBusy;
    bool                                    m_ApplyEnabled;
    bool                                    m_CancelEnabled; 
    #endregion

    #region Constants
    const string PART_PANEL                                     = "PART_Panel";
    const string PART_PROGRESSBAR                               = "PART_ProgressBar";
    const string PART_MESSAGETEXT                               = "PART_MessageText";
    const string PART_APPLYBUTTON                               = "PART_ApplyButton";
    const string PART_CANCELBUTTON                              = "PART_CancelButton";
    const string PART_CONTENTBUSY                               = "PART_ContentBusy";
    #endregion
  };
  //---------------------------//

}  // namespace