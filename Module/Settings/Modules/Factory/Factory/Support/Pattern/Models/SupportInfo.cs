/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Factory.Support.Pattern.Models
{
  public class TSupportInfo
  {
    #region Property
    public string PropertyName
    {
      get;
    }

    public string PropertyDescription
    {
      get;
    }

    public System.Windows.Style PropertyIconStyle
    {
      get;
    }

    public string PropertyValueString
    {
      get
      {
        return (PropertyValue.ToString ());
      }
    }

    public object PropertyValue
    {
      get;
      set;
    }

    public string ErrorMessage
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TSupportInfo (string propertyIconStyle, string propertyName, string propertyDescription)
    {
      PropertyName = propertyName;
      PropertyDescription = propertyDescription;

      PropertyIconStyle = ((System.Windows.Style) System.Windows.Application.Current.FindResource (propertyIconStyle));
    }
    #endregion

    #region Members
    public void Update (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        if (PropertyName.Equals ("ColumnWidth")) {
          PropertyValue = item.SettingsModel.ColumnWidth;
        }
      }
    }

    public bool Validate ()
    {
      var intValue = int.Parse (PropertyValue.ToString ());

      if ((intValue >= 260) && (intValue <= 460)) {
        ErrorMessage = string.Empty;
        return (true);
      }

      ErrorMessage = $"{intValue}: Value dismatch! ColumnWidth value must be 260 - 460";

      return (false);
    }

    public void Request (Server.Models.Component.TEntityAction action)
    {
      action.ModelAction.SettingsModel.ColumnWidth = int.Parse (PropertyValue.ToString ());
    }
    #endregion
  };
  //---------------------------//

}  // namespace
