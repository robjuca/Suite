/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;

using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.Models
{
  public class TFactorySupportModel
  {
    #region Property
    public ObservableCollection<TSupportInfo> SupportInfoCollection
    {
      get;
      private set;
    }

    public Visibility ErrorPanelVisibility
    {
      get;
      private set;
    }

    public string ErrorPanelMessage
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TFactorySupportModel ()
    {
      SupportInfoCollection = new ObservableCollection<TSupportInfo>
      {
        new TSupportInfo ("SettingsSupportIcon", "ColumnWidth", "('mini' style column width [260 - 460 pixel])")
      };

      ErrorPanelVisibility = Visibility.Hidden;
      ErrorPanelMessage = string.Empty;
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem item)
    {
      item.ThrowNull ();

      foreach (var info in SupportInfoCollection) {
        info.Update (item);
      }
    }

    internal bool Validate ()
    {
      CleanupErrorPanelMessage ();

      foreach (var info in SupportInfoCollection) {
        if (info.Validate ().IsFalse ()) {
          ShowErrorPanelMessage (info.ErrorMessage);

          return (false);
        }
      }

      return (true);
    }

    internal void Request (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        foreach (var info in SupportInfoCollection) {
          info.Request (action);
        }
      }
    }
    #endregion

    #region Support
    void ShowErrorPanelMessage (string error)
    {
      ErrorPanelVisibility = Visibility.Visible;
      ErrorPanelMessage = error;
    }

    void CleanupErrorPanelMessage ()
    {
      ErrorPanelVisibility = Visibility.Hidden;
      ErrorPanelMessage = string.Empty;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
