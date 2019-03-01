/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
using System.Windows.Controls;
//---------------------------//

namespace Shared.Resources
{
  [TemplatePart (Name = PART_ERROR_BOX, Type = typeof (Border))]
  [TemplatePart (Name = PART_ERROR_TITLE, Type = typeof (TextBlock))]
  [TemplatePart (Name = PART_ERROR_CAPTION, Type = typeof (TextBlock))]
  [TemplatePart (Name = PART_ERROR_MESSAGE, Type = typeof (TextBox))]
  public sealed class TErrorBox : Control
  {
    #region Property
    public int ErrorBoxWidth
    {
      get
      {
        return (int) GetValue (ErrorBoxWidthProperty);
      }

      set
      {
        SetValue (ErrorBoxWidthProperty, value);
      }
    }

    public int ErrorBoxHeight
    {
      get
      {
        return (int) GetValue (ErrorBoxHeightProperty);
      }

      set
      {
        SetValue (ErrorBoxHeightProperty, value);
      }
    }

    public string ErrorTitle
    {
      get
      {
        return (string) GetValue (ErrorTitleProperty);
      }

      set
      {
        SetValue (ErrorTitleProperty, value);
      }
    }

    public string ErrorCaption
    {
      get
      {
        return (string) GetValue (ErrorCaptionProperty);
      }

      set
      {
        SetValue (ErrorCaptionProperty, value);
      }
    }

    public string ErrorMessage
    {
      get
      {
        return (string) GetValue (ErrorMessageProperty);
      }

      set
      {
        SetValue (ErrorMessageProperty, value);
      }
    }
    #endregion

    #region Dependency Property
    public static readonly DependencyProperty ErrorBoxWidthProperty =
        DependencyProperty.Register ("ErrorBoxWidth", typeof (int), typeof (TErrorBox),
        new FrameworkPropertyMetadata (0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ErrorBoxWidthPropertyChanged));

    public static readonly DependencyProperty ErrorBoxHeightProperty =
        DependencyProperty.Register ("ErrorBoxHeight", typeof (int), typeof (TErrorBox),
        new FrameworkPropertyMetadata (0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ErrorBoxHeightPropertyChanged));

    public static readonly DependencyProperty ErrorTitleProperty =
        DependencyProperty.Register ("ErrorTitle", typeof (string), typeof (TErrorBox),
        new FrameworkPropertyMetadata (string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ErrorTitlePropertyChanged));

    public static readonly DependencyProperty ErrorCaptionProperty =
        DependencyProperty.Register ("ErrorCaption", typeof (string), typeof (TErrorBox),
        new FrameworkPropertyMetadata (string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ErrorCaptionPropertyChanged));

    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register ("ErrorMessage", typeof (string), typeof (TErrorBox), 
        new FrameworkPropertyMetadata (string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ErrorMessagePropertyChanged));
    #endregion

    #region Constructor
    public TErrorBox ()
    {
      DefaultStyleKey = typeof (TErrorBox);
    }
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      base.OnApplyTemplate ();

      if (GetTemplateChild (PART_ERROR_BOX) is Border box) {
        box.Width = BoxWidth;
        box.Height = BoxHeight;
      }

      if (GetTemplateChild (PART_ERROR_TITLE) is TextBlock textTitle) {
        textTitle.Text = BoxErrorTitle;
      }

      if (GetTemplateChild (PART_ERROR_CAPTION) is TextBlock textCaption) {
        textCaption.Text = BoxErrorCaption;
      }

      if (GetTemplateChild (PART_ERROR_MESSAGE) is TextBox textMessage) {
        textMessage.Text = BoxErrorMessage;
      }
    }
    #endregion

    #region Event
    static void ErrorBoxWidthPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TErrorBox errorBox) {
        if (e.NewValue is int width) {
          if (errorBox.GetTemplateChild (PART_ERROR_BOX) is Border box) {
            box.Width = width;
          }

          else {
            errorBox.BoxWidth = width;
          }
        }
      }
    }

    static void ErrorBoxHeightPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TErrorBox errorBox) {
        if (e.NewValue is int height) {
          if (errorBox.GetTemplateChild (PART_ERROR_BOX) is Border box) {
            box.Height = height;
          }

          else {
            errorBox.BoxHeight = height;
          }
        }
      }
    }

    static void ErrorTitlePropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TErrorBox errorBox) {
        if (e.NewValue is string title) {
          if (errorBox.GetTemplateChild (PART_ERROR_TITLE) is TextBlock text) {
            text.Text= title;
          }

          else {
            errorBox.BoxErrorTitle = title;
          }
        }
      }
    }

    static void ErrorCaptionPropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TErrorBox errorBox) {
        if (e.NewValue is string caption) {
          if (errorBox.GetTemplateChild (PART_ERROR_CAPTION) is TextBlock text) {
            text.Text = caption;
          }

          else {
            errorBox.BoxErrorCaption = caption;
          }
        }
      }
    }

    static void ErrorMessagePropertyChanged (DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if (source is TErrorBox errorBox) {
        if (e.NewValue is string message) {
          if (errorBox.GetTemplateChild (PART_ERROR_MESSAGE) is TextBox text) {
            text.Text = message;
          }

          else {
            errorBox.BoxErrorMessage = message;
          }
        }
      }
    }
    #endregion

    #region Property
    internal int BoxWidth
    {
      get; set;
    }

    internal int BoxHeight
    {
      get; set;
    }

    internal string BoxErrorTitle
    {
      get; set;
    }

    internal string BoxErrorCaption
    {
      get; set;
    }

    internal string BoxErrorMessage
    {
      get; set;
    } 
    #endregion

    #region Constants
    const string PART_ERROR_BOX                       = "PART_ErrorBox";
    const string PART_ERROR_TITLE                     = "PART_ErrorTitle";
    const string PART_ERROR_CAPTION                   = "PART_ErrorCaption";
    const string PART_ERROR_MESSAGE                   = "PART_ErrorMessage";
    #endregion
  };
  //---------------------------//

}  // namespace