/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Component;

using Shared.Types;
//---------------------------//

namespace Module.Factory.Pattern.Models
{
  public class TFactoryDisplayModel
  {
    #region Property
    public int ImageWidth
    {
      get;
      set;
    }

    public int ImageHeight
    {
      get;
      set;
    }

    public string DesiredSize
    {
      get;
      set;
    }

    public string CurrentSize
    {
      get;
      set;
    }

    public byte [] Image
    {
      get;
      set;
    }

    public Visibility DistortedPictureVisibility
    {
      get;
      set;
    }

    public bool Distorted
    {
      get;
      private set;
    }

    public int CurrentWidth
    {
      get;
      private set;
    }

    public int CurrentHeight
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TFactoryDisplayModel ()
    {
      Distorted = false;
      DistortedPictureVisibility = Visibility.Collapsed;
    }
    #endregion

    #region Members
    internal void PropertySelect (string propertyName, TEntityAction action)
    {
      if (propertyName.Equals ("StyleProperty")) {
        SelectStyle (action);
      }

      if (propertyName.Equals ("ImageProperty")) {
        SelectImage (action);
      }

      if (propertyName.Equals ("FrameImageCleanup")) {
        CleanupImage ();
      }

      if (propertyName.Equals ("all")) {
        SelectStyle (action);
        SelectImage (action);
      }
    }

    internal void RequestReport (TReportData reportData)
    {
      reportData.ThrowNull ();

      reportData.Select (false, Distorted);
    }

    internal void Cleanup ()
    {
      CleanupImage ();
    }
    #endregion

    #region Support
    void CleanupImage ()
    {
      Image = null;
      Distorted = false;
      CurrentSize = string.Empty;
      DistortedPictureVisibility = Visibility.Collapsed;
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      action.ModelAction.ExtensionImageModel.Image = Image;
      action.ModelAction.ExtensionImageModel.Width = CurrentWidth;
      action.ModelAction.ExtensionImageModel.Height = CurrentHeight;
      action.ModelAction.ExtensionImageModel.Distorted = Distorted;
    }

    void ValidateDistortedImage ()
    {
      if (Image.IsNull ()) {
        Distorted = false;
        DistortedPictureVisibility = Visibility.Collapsed;
      }

      else {
        Distorted = (ImageWidth.NotEquals (CurrentWidth) || ImageHeight.NotEquals (CurrentHeight));
        DistortedPictureVisibility = Distorted ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    void SelectStyle (TEntityAction action)
    {
      ImageWidth = action.ModelAction.ExtensionLayoutModel.Width;
      ImageHeight = action.ModelAction.ExtensionLayoutModel.Height;

      DesiredSize = $"style: {action.ModelAction.ExtensionLayoutModel.Width} x {action.ModelAction.ExtensionLayoutModel.Height} - {action.ModelAction.ExtensionLayoutModel.Style}";

      ValidateDistortedImage ();
    }

    void SelectImage (TEntityAction action)
    {
      CurrentWidth = action.ModelAction.ExtensionImageModel.Width;
      CurrentHeight = action.ModelAction.ExtensionImageModel.Height;
      Image = action.ModelAction.ExtensionImageModel.Image;

      CurrentSize = $"current: {CurrentWidth} x {CurrentHeight}";

      ValidateDistortedImage ();

      action.ModelAction.ExtensionImageModel.Distorted = Distorted;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
