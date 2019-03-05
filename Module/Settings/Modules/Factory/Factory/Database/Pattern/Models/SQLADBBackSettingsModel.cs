/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;

using rr.Library.Types;
//---------------------------//

namespace Module.Settings.Factory.Database.Pattern.Models
{
  public class TSQLADBBackSettingsModel
  {
    #region Property
    public TDatabaseAuthentication DatabaseAuthentication
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
    public TSQLADBBackSettingsModel ()
    {
      DatabaseAuthentication = TDatabaseAuthentication.Create (TAuthentication.SQL);

      CheckVisibility = Visibility.Hidden;
    }
    #endregion

    #region Members
    internal void Apply ()
    {
      DatabaseAuthentication.Apply ();

      CheckVisibility = Visibility.Visible;
    }

    internal void ClearCheck ()
    {
      CheckVisibility = Visibility.Collapsed;
    }

    internal void Populate (TDatabaseAuthentication data)
    {
      if (data != null) {
        DatabaseAuthentication.Change (data);
      }
    }
    #endregion
  };
  //---------------------------//

}