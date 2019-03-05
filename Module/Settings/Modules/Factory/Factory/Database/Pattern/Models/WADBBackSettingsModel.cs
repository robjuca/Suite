/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Types;
//---------------------------//

namespace Module.Settings.Factory.Database.Pattern.Models
{
  public class TWADBBackSettingsModel
  {
    #region Property
    public TDatabaseAuthentication DatabaseConnectionData
    {
      get;
    }

    public Visibility CheckVisibility
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TWADBBackSettingsModel ()
    {
      DatabaseConnectionData = TDatabaseAuthentication.Create (TAuthentication.Windows);

      CheckVisibility = Visibility.Hidden;
    }
    #endregion

    #region Members
    internal void Apply ()
    {
      DatabaseConnectionData.Apply ();

      CheckVisibility = Visibility.Visible;
    }

    internal void ClearCheck ()
    {
      CheckVisibility = Visibility.Collapsed;
    }

    internal void Populate (TDatabaseAuthentication data)
    {
      if (data != null) {
        DatabaseConnectionData.CopyFrom (data);
      }
    }
    #endregion
  };
  //---------------------------//

}