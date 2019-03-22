/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using Shared.Types;
//---------------------------//

namespace Shared.Gadget.Document
{
  public abstract class TComponentControlBase : ContentControl
  {
    #region Dependency Property
    public static readonly DependencyProperty ComponentControlModelProperty =
      DependencyProperty.Register ("Model", typeof (TComponentControlModel), typeof (TComponentControlBase),
      new FrameworkPropertyMetadata (TComponentControlModel.CreateDefault, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ModelPropertyChanged));
    #endregion

    #region Property

    public string InfoReport
    {
      get
      {
        return ($"style: {Model.HorizontalStyleString}, {Model.VerticalStyleString} ({Model.Width} x {Model.Height})");
      }
    }

    public string ImageInfoReport
    {
      get;
      private set;
    }
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
    #endregion

    #region Constructor
    public TComponentControlBase ()
    {
      Background = Brushes.White;

      m_Image = new Image ()
      {
        Stretch = Stretch.Fill
      };

      m_FullGrid = new Grid ();

      #region Content (header, paragraph, footer)
      #region header
      m_Header = new RichTextBox ()
      {
        Padding = new Thickness (3),
        BorderThickness = new Thickness (.5, 0, .5, 0),
        Background = Brushes.Transparent,
      };

      m_Header.SetValue (DockPanel.DockProperty, Dock.Top);
      #endregion

      #region footer
      m_Footer = new RichTextBox ()
      {
        Padding = new Thickness (3),
        BorderThickness = new Thickness (.5, 0, .5, 0),
        Background = Brushes.Transparent,
      };

      m_Footer.SetValue (DockPanel.DockProperty, Dock.Bottom);
      #endregion

      #region paragraph
      m_Paragraph = new RichTextBox ()
      {
        Padding = new Thickness (3),
        BorderThickness = new Thickness (.5),
        BorderBrush = Brushes.LightGray,
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch,
        Background = Brushes.Transparent
      };
      #endregion

      m_ContentPanel = new DockPanel ()
      {
        LastChildFill = true,
        Background = Brushes.Transparent,
      };

      m_ContentPanel.Children.Add (m_Header);
      m_ContentPanel.Children.Add (m_Footer);
      m_ContentPanel.Children.Add (m_Paragraph);
      #endregion

      #region design info
      m_DocumentInfo = new TextBlock ()
      {
        HorizontalAlignment = HorizontalAlignment.Center,
        Margin = new Thickness (0, 0, 0, 5)
      };

      m_DocumentInfo.SetValue (Grid.RowProperty, 0);

      m_ImageInfo = new TextBlock ()
      {
        HorizontalAlignment = HorizontalAlignment.Center,
        Margin = new Thickness (0, 0, 0, 15)
      };

      m_ImageInfo.SetValue (Grid.RowProperty, 1);
      #endregion

      #region Document link
      m_DocumentLinkButton = new Button ();
      m_DocumentLinkButton.Click += OnDocumentLinkButtonClick;

      m_DocumentLinkBorder = new Border () { Visibility = Visibility.Collapsed };
      m_DocumentLinkBorder.Child = m_DocumentLinkButton;
      #endregion

      #region border panel
      m_DocumentBorder = new Border ()
      {
        Margin = new Thickness (0),
        Padding = new Thickness (0)
      };

      m_BorderPanel = new DockPanel ()
      {
        LastChildFill = true
      };

      m_DocumentBorder.Child = m_BorderPanel;

      var DocumentGrid = new Grid ();
      DocumentGrid.SetValue (Grid.RowProperty, 3);
      DocumentGrid.Children.Add (m_DocumentBorder);
      DocumentGrid.Children.Add (m_DocumentLinkBorder);
      #endregion

      #region preview
      m_PreviewButton = new ToggleButton ()
      {
        Content = "preview",
        Width = 60,
        BorderThickness = new Thickness (0)
      };

      m_PreviewButton.SetValue (Grid.RowProperty, 2);
      m_PreviewButton.Checked += OnPreviewButtonChecked;
      m_PreviewButton.Unchecked += OnPreviewButtonUnchecked;
      #endregion

      var grid = new Grid ();
      grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 0 Document info
      grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1 image info
      grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 2 preview button
      grid.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Star) }); // row 3 Document

      //grid.Children.Add (m_DocumentInfo);
      //grid.Children.Add (m_ImageInfo);
      //grid.Children.Add (m_PreviewButton);
      grid.Children.Add (DocumentGrid);

      AddChild (grid);
    }

    protected TComponentControlBase (TControlMode mode)
      : this ()
    {
      ControlMode = mode;

      m_Header.IsReadOnly = ControlMode.Equals (TControlMode.Display);

      m_Footer.IsReadOnly = ControlMode.Equals (TControlMode.Display);

      m_Paragraph.IsReadOnly = ControlMode.Equals (TControlMode.Display);
      m_Paragraph.BorderBrush = ControlMode.Equals (TControlMode.Display) ? Brushes.White: m_Paragraph.BorderBrush;
    }
    #endregion

    #region Members
    public void RefreshDesign ()
    {
      Width = Model.Width;

      // apply Document style
      m_DocumentBorder.Width = Model.Width;
      m_DocumentBorder.Height = Model.Height;

      m_DocumentLinkBorder.Width = Model.Width;
      m_DocumentLinkBorder.Height = Model.Height;

      #region image
      m_Image.Width = Model.ImageGeometry.Size.Width;
      m_Image.Height = Model.ImageGeometry.Size.Height;

      if (Model.Image.IsNull ()) {
        m_Image.Source = null;
      }

      if (Model.Image.NotNull () && (Model.PropertyName.Equals ("ImageProperty") || Model.PropertyName.Equals ("all"))) {
        var imageSource = rr.Library.Helper.THelper.ByteArrayToBitmapImage (Model.Image).Clone ();
        m_CurrentImageWidth = imageSource.PixelWidth;
        m_CurrentImageHeight = imageSource.PixelHeight;

        m_Image.Source = imageSource;
      }

      ImageInfoReport = string.Empty;
      #endregion

      #region visibility
      if (Model.PropertyName.Equals ("header visibility") || Model.PropertyName.Equals ("all")) {
        m_Header.Visibility = Model.HeaderVisibility.Equals ("visible") ? Visibility.Visible : Visibility.Collapsed;
      }

      if (Model.PropertyName.Equals ("footer visibility") || Model.PropertyName.Equals ("all")) {
        m_Footer.Visibility = Model.FooterVisibility.Equals ("visible") ? Visibility.Visible : Visibility.Collapsed;
      }
      #endregion

      m_FullGrid.Children.Clear ();
      m_BorderPanel.Children.Clear ();

      //Document image position
      if (string.IsNullOrEmpty (Model.ImageGeometry.Position.Position).IsFalse ()) {
        var position = Enum.Parse (typeof (Positions.Image), Model.ImageGeometry.Position.Position.ToLower ());

        switch (position) {
          case Positions.Image.left:
            m_Image.SetValue (DockPanel.DockProperty, Dock.Left);

            m_BorderPanel.Children.Add (m_Image);
            m_BorderPanel.Children.Add (m_ContentPanel);
            break;

          case Positions.Image.right:
            m_Image.SetValue (DockPanel.DockProperty, Dock.Right);

            m_BorderPanel.Children.Add (m_Image);
            m_BorderPanel.Children.Add (m_ContentPanel);
            break;

          case Positions.Image.top:
            m_Image.SetValue (DockPanel.DockProperty, Dock.Top);

            m_BorderPanel.Children.Add (m_Image);
            m_BorderPanel.Children.Add (m_ContentPanel);
            break;

          case Positions.Image.bottom:
            m_Image.SetValue (DockPanel.DockProperty, Dock.Bottom);

            m_BorderPanel.Children.Add (m_Image);
            m_BorderPanel.Children.Add (m_ContentPanel);
            break;

          case Positions.Image.full:
            m_FullGrid.Children.Add (m_Image);
            m_FullGrid.Children.Add (m_ContentPanel);

            m_BorderPanel.Children.Add (m_FullGrid);
            break;

          case Positions.Image.none:
            m_BorderPanel.Children.Add (m_ContentPanel);
            break;
        }
      }

      // image
      ImageInfoReport = $"image position: {Model.ImageGeometry.Position.Position} ({Model.ImageGeometry.Size.Width} x {Model.ImageGeometry.Size.Height})";
      Model.ImageDistorted = false;

      if (Model.ImageGeometry.Size.Width.NotEquals (m_CurrentImageWidth) || Model.ImageGeometry.Size.Height.NotEquals (m_CurrentImageHeight)) {
        if ((m_CurrentImageWidth > 0) || (m_CurrentImageHeight > 0)) {
          Model.ImageDistorted = true;
          ImageInfoReport += $" [ DISTORTED ({m_CurrentImageWidth} x {m_CurrentImageHeight}) ]";
        }
      }

      //info
      m_DocumentInfo.Text = InfoReport;
      m_ImageInfo.Text = ImageInfoReport;

      Model.InfoReport = InfoReport;
      Model.ImageInfoReport = ImageInfoReport;

      if (Model.PropertyName.Equals ("Document link") || Model.PropertyName.Equals ("all")) {
        m_DocumentLinkButton.Tag = Model.ExternalLink;
      }

      if (Model.PropertyName.Equals ("all")) {
        SetRtf ();
      }
    }

    public string RequestHeader (bool clear = true)
    {
      var text = string.Empty;

      var tr = new System.Windows.Documents.TextRange (m_Header.Document.ContentStart, m_Header.Document.ContentEnd);

      using (System.IO.MemoryStream ms = new System.IO.MemoryStream ()) {
        tr.Save (ms, DataFormats.Rtf);
        text = System.Text.Encoding.UTF8.GetString (ms.ToArray ());
      }

      if (clear) {
        m_Header.Document.Blocks.Clear ();
      }

      return (text);
    }

    public string RequestFooter (bool clear = true)
    {
      var text = string.Empty;

      var tr = new System.Windows.Documents.TextRange (m_Footer.Document.ContentStart, m_Footer.Document.ContentEnd);

      using (System.IO.MemoryStream ms = new System.IO.MemoryStream ()) {
        tr.Save (ms, DataFormats.Rtf);
        text = System.Text.Encoding.UTF8.GetString (ms.ToArray ());
      }

      if (clear) {
        m_Footer.Document.Blocks.Clear ();
      }

      return (text);
    }

    public string RequestParagraph (bool clear = true)
    {
      var text = string.Empty;

      var tr = new System.Windows.Documents.TextRange (m_Paragraph.Document.ContentStart, m_Paragraph.Document.ContentEnd);

      using (System.IO.MemoryStream ms = new System.IO.MemoryStream ()) {
        tr.Save (ms, DataFormats.Rtf);
        text = System.Text.Encoding.UTF8.GetString (ms.ToArray ());
      }

      if (clear) {
        m_Paragraph.Document.Blocks.Clear ();
      }

      return (text);
    }

    public bool RequestImageDistorted ()
    {
      return (Model.ImageDistorted);
    }

    public void ClearRtf ()
    {
      m_Header.Document.Blocks.Clear ();
      m_Footer.Document.Blocks.Clear ();
      m_Paragraph.Document.Blocks.Clear ();
    }

    public void Cleanup ()
    {
      Model = null;
      Model = TComponentControlModel.CreateDefault;

      m_CurrentImageHeight = 0;
      m_CurrentImageWidth = 0;

      ModelValidated = true;
    }
    #endregion

    #region Event
    void OnPreviewButtonUnchecked (object sender, RoutedEventArgs e)
    {
      m_Header.BorderThickness = new Thickness (.5, 0, .5, 0);
      m_Header.IsReadOnly = false;

      m_Footer.BorderThickness = new Thickness (.5, 0, .5, 0);
      m_Footer.IsReadOnly = false;

      m_Paragraph.BorderThickness = new Thickness (.5);
      m_Paragraph.IsReadOnly = false;

      m_DocumentLinkBorder.Visibility = Visibility.Collapsed;
    }

    void OnPreviewButtonChecked (object sender, RoutedEventArgs e)
    {
      m_Header.BorderThickness = new Thickness (0);
      m_Header.IsReadOnly = true;

      m_Footer.BorderThickness = new Thickness (0);
      m_Footer.IsReadOnly = true;
    
      m_Paragraph.BorderThickness = new Thickness (0);
      m_Paragraph.IsReadOnly = true;

      if (m_DocumentLinkButton.Tag != null) {
        var link = m_DocumentLinkButton.Tag as string;

        if (string.IsNullOrEmpty (link) == false) {
          m_DocumentLinkBorder.Visibility = Visibility.Visible;
        }
      }
    }

    void OnDocumentLinkButtonClick (object sender, RoutedEventArgs e)
    {
      var link = m_DocumentLinkButton.Tag as string;

      if (string.IsNullOrEmpty (link) == false) {
        var uri = string.Format ("http://www.{0}", link);
        System.Diagnostics.Process.Start (new System.Diagnostics.ProcessStartInfo (uri));
        e.Handled = true;
      }
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

    #region Property
    internal bool ModelValidated
    {
      get;
      set;
    }

    TControlMode ControlMode
    {
      get;
      set;
    }
    #endregion

    #region Fields
    Border                                  m_DocumentBorder;
    Border                                  m_DocumentLinkBorder;
    Button                                  m_DocumentLinkButton;
    DockPanel                               m_ContentPanel;
    DockPanel                               m_BorderPanel;
    Image                                   m_Image;
    RichTextBox                             m_Header;
    RichTextBox                             m_Footer;
    RichTextBox                             m_Paragraph;
    TextBlock                               m_DocumentInfo;
    TextBlock                               m_ImageInfo;
    Grid                                    m_FullGrid;
    ToggleButton                            m_PreviewButton;
    int                                     m_CurrentImageWidth;
    int                                     m_CurrentImageHeight;
    #endregion

    #region Support
    void SetRtf ()
    {
      ClearRtf ();

      System.Windows.Documents.TextRange textRange = null;
      System.IO.MemoryStream stream = null;

      if (string.IsNullOrEmpty (Model.RtfHeader).IsFalse ()) {
        stream = new System.IO.MemoryStream (System.Text.Encoding.Default.GetBytes (Model.RtfHeader));

        textRange = new System.Windows.Documents.TextRange (m_Header.Document.ContentStart, m_Header.Document.ContentEnd);
        textRange.Load (stream, DataFormats.Rtf);

        stream.Close ();
      }

      if (string.IsNullOrEmpty (Model.RtfFooter).IsFalse ()) {
        stream = new System.IO.MemoryStream (System.Text.Encoding.Default.GetBytes (Model.RtfFooter));

        textRange = new System.Windows.Documents.TextRange (m_Footer.Document.ContentStart, m_Footer.Document.ContentEnd);
        textRange.Load (stream, DataFormats.Rtf);

        stream.Close ();
      }

      if (string.IsNullOrEmpty (Model.RtfParagraph).IsFalse ()) {
        stream = new System.IO.MemoryStream (System.Text.Encoding.Default.GetBytes (Model.RtfParagraph));

        textRange = new System.Windows.Documents.TextRange (m_Paragraph.Document.ContentStart, m_Paragraph.Document.ContentEnd);
        textRange.Load (stream, DataFormats.Rtf);

        stream.Close ();
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace