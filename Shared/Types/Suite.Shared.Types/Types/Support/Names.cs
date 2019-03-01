/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public static class TNames
  {
    #region Property
    public static string SettingsIniFileName
    {
      get
      {
        return (SettingsINI);
      }
    } 
    #endregion

    // Settings INI file name
    const string                            SettingsINI = "Settings.ini";
  };
  //---------------------------//

}  // namespace