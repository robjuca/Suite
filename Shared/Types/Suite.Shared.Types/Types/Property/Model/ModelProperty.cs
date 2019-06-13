/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Types;

using Server.Models.Component;
//---------------------------//

namespace Shared.Types
{
  public class TModelProperty : NotificationObject
  {
    #region Property
    public TPropertyComponentModel ComponentModel
    {
      get;
      set;
    }

    public TPropertyExtensionModel ExtensionModel
    {
      get;
      set;
    }

    public bool IsInfoEnabled
    {
      get;
      set;
    }

    public string Name
    {
      get
      {
        return (ComponentModel.NameProperty);
      }
    }

    public bool Distorted
    {
      get;
      private set;
    }

    public bool IsEnabledApply
    {
      get;
      set;
    }

    public bool IsEnabledCancel
    {
      get;
      set;
    }

    public bool ShowPanel
    {
      get;
      set;
    }

    public string MessagePanel
    {
      get;
      set;
    }

    public bool IsActiveProgress
    {
      get;
      set;
    }

    public Visibility BusyVisibility
    {
      get;
      set;
    }

    public Visibility DistortedVisibility
    {
      get;
      set;
    }

    public bool IsBusy
    {
      get;
      private set;
    }

    public Guid Id
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TModelProperty (Server.Models.Infrastructure.TCategory modelCategory)
      : this ()
    {
      ExtensionModel.SelectModelCategory (modelCategory);
      ExtensionModel.ValidateModel ();
    }

    TModelProperty ()
    {
      ComponentModel = TPropertyComponentModel.CreateDefault;
      ComponentModel.PropertyChanged += OnPropertyChanged;

      ExtensionModel = TPropertyExtensionModel.CreateDefault;
      ExtensionModel.PropertyChanged += OnPropertyChanged;

      BusyVisibility = Visibility.Collapsed;
      DistortedVisibility = Visibility.Collapsed;

      Distorted = false;
      Id = Guid.Empty;

      IsInfoEnabled = true;
      IsBusy = false;
      ShowPanel = false;
    }
    #endregion

    #region Members
    public void Initialize ()
    {
      ExtensionModel.Initialize ();
    }

    public void RequestModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        ComponentModel.RequestModel (action);
        ExtensionModel.RequestModel (action);

        action.Id = Id;
        action.ModelAction.ComponentInfoModel.Id = Id;

        action.ModelAction.ComponentStatusModel.Id = Id;
        action.ModelAction.ComponentStatusModel.Busy = IsBusy;
      }
    }

    public void SelectModel (TEntityAction action)
    {
      if (action.NotNull ()) {
        // DO NOT CHANGE THIS ORDER
        ExtensionModel.SelectModel (action);
        ComponentModel.SelectModel (action);

        IsBusy = action.ModelAction.ComponentStatusModel.Busy;

        BusyVisibility = IsBusy ? Visibility.Visible : Visibility.Collapsed;
        IsInfoEnabled = IsBusy.IsFalse ();

        Id = action.Id;

        IsEnabledApply = string.IsNullOrEmpty (Name.Trim ()).IsFalse ();
        IsEnabledCancel = Id.NotEmpty ();
      }
    }

    public void SelectReport (TReportData reportData)
    {
      if (reportData.NotNull ()) {
        Distorted = reportData.Distorted;

        DistortedVisibility = Distorted ? Visibility.Visible : Visibility.Collapsed;
        IsEnabledApply = Distorted ? false : string.IsNullOrEmpty (Name.Trim ()).IsFalse ();

        ExtensionModel.SelectReport (reportData);
      }
    }

    public void Cleanup ()
    {
      ComponentModel.Cleanup ();
      ExtensionModel.Cleanup ();

      IsEnabledApply = false;
      IsEnabledCancel = false;
      IsInfoEnabled = true;

      Distorted = false;

      BusyVisibility = Visibility.Collapsed;
      DistortedVisibility = Visibility.Collapsed;

      Id = Guid.Empty;

      ClearPanels ();
      Initialize ();
    }

    public void ImageCleanup ()
    {
      ExtensionModel.ImageCleanup ();
    }

    public void ValidateApplyCommand ()
    {
      IsEnabledApply = string.IsNullOrEmpty (Name.Trim ()).IsFalse ();
    }

    public void ShowPanels ()
    {
      ShowPanel = true;
      IsActiveProgress = true;
      MessagePanel = "applying...";
    }

    public void ClearPanels ()
    {
      ShowPanel = false;
      IsActiveProgress = false;
      MessagePanel = string.Empty;
    }
    #endregion

    #region Event
    void OnPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName.Equals ("NameProperty")) {
        ValidateApplyCommand ();
      }

      if (e.PropertyName.Equals ("FrameImageCleanup")) {
        ImageCleanup ();
      }

      RaisePropertyChanged (e.PropertyName);
    }
    #endregion

    #region Static
    public static TModelProperty Create (Server.Models.Infrastructure.TCategory modelCategory) => new TModelProperty (modelCategory);
    #endregion
  };
  //---------------------------//

}  // namespace