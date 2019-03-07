/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;
//---------------------------//

namespace Module.Settings.Factory.Database.Pattern.Models
{
  public class TFactoryDatabaseModel
  {
    #region Property
    public TDatabaseAuthentication AuthenticationSQL
    {
      get;
      private set;
    }

    public TDatabaseAuthentication AuthenticationWindows
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TFactoryDatabaseModel ()
    {
      AuthenticationSQL = TDatabaseAuthentication.CreateDefault;
      AuthenticationWindows = TDatabaseAuthentication.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (TDatabaseAuthentication authentication)
    {
      switch (authentication.Authentication) {
        case TAuthentication.SQL:
          AuthenticationSQL.CopyFrom (authentication);
          break;

        case TAuthentication.Windows:
          AuthenticationWindows.CopyFrom (authentication);
          break;
      }
    }

    internal TDatabaseAuthentication Request (TAuthentication authentication)
    {
      switch (authentication) {
        case TAuthentication.SQL:
          return (AuthenticationSQL);

        case TAuthentication.Windows:
          return (AuthenticationWindows);
      }

      return (TDatabaseAuthentication.CreateDefault);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
