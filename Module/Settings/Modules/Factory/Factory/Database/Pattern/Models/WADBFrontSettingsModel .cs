/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;
//---------------------------//

namespace Module.Settings.Factory.Database.Pattern.Models
{
  public class TWADBFrontSettingsModel
  {
    #region Property
    public string DatabaseConnection1
    {
      get;
      private set;
    }

    public bool IsCheckedAuthentication
    {
      get;
      set;
    }

    public bool IsEnabled
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TWADBFrontSettingsModel ()
    {
      DatabaseAuthentication = TDatabaseAuthentication.Create (TAuthentication.Windows);

      IsEnabled = true;
    }
    #endregion

    #region Members
    internal void Populate (TDatabaseAuthentication data)
    {
      if (data != null) {
        if (data.Authentication.Equals (DatabaseAuthentication.Authentication)) {
          DatabaseAuthentication.CopyFrom (data);

          DatabaseConnection1 = $"Data Server = {data.DatabaseServer};  Initial Catalog = {data.DatabaseName}";
        }

        IsCheckedAuthentication = DatabaseAuthentication.IsActive;
      }
    }

    internal void FactoryEnter ()
    {
      IsEnabled = false;
    }

    internal void FactoryLeave ()
    {
      IsEnabled = true;
    }
    #endregion

    #region Property
    internal TDatabaseAuthentication DatabaseAuthentication
    {
      get;
      private set;
    } 
    #endregion
  };
  //---------------------------//

}