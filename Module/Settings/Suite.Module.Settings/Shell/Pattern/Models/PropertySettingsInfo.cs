/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.ObjectModel;
//---------------------------//

namespace Module.Settings.Shell.Pattern.Models
{
  public class TPropertySettingsInfo
  {
    #region Property
    public string PropertyName
    {
      get;
    }

    public System.Windows.Style PropertyIconStylePrimary
    {
      get;
    }

    public System.Windows.Style PropertyIconStyleSecondary
    {
      get;
    }

    public Collection <TPropertyValueInfo> PropertyValueCollection
    {
      get;
    }
    #endregion

    #region Constructor
    TPropertySettingsInfo ()
    {
      PropertyValueCollection = new Collection<TPropertyValueInfo> ();
    }

    public TPropertySettingsInfo (string propertyIconStylePrimary, string propertyName)
      : this ()
    {
      PropertyName = propertyName;

      PropertyIconStylePrimary = ((System.Windows.Style) System.Windows.Application.Current.FindResource (propertyIconStylePrimary));
    }

    public TPropertySettingsInfo (string propertyIconStylePrimary, string propertyIconStyleSecondary, string propertyName)
      : this ()
    {
      PropertyName = propertyName;

      PropertyIconStylePrimary = ((System.Windows.Style) System.Windows.Application.Current.FindResource (propertyIconStylePrimary));
      PropertyIconStyleSecondary = ((System.Windows.Style) System.Windows.Application.Current.FindResource (propertyIconStyleSecondary));
    }
    #endregion

    #region Members
    public void AddPropertyValue (TPropertyValueInfo propertyValue)
    {
      PropertyValueCollection.Add (propertyValue);
    } 
    #endregion
  };
  //---------------------------//

  //----- TPropertyValueInfo
  public class TPropertyValueInfo
  {
    #region Property
    public System.Windows.Style PropertyValueIconStyle
    {
      get;
    }
    public string ValueString
    {
      get;
    }
    #endregion

    #region Constructor
    public TPropertyValueInfo (string valueString)
    {
      ValueString = valueString;
    }

    public TPropertyValueInfo (string propertyValueIconStyle, string valueString)
    {
      ValueString = valueString;

      PropertyValueIconStyle = ((System.Windows.Style) System.Windows.Application.Current.FindResource (propertyValueIconStyle));
    }
    #endregion
  };
  //---------------------------//

}  // namespace