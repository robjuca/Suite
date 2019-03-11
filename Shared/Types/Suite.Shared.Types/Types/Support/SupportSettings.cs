/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using rr.Library.Helper;
//---------------------------//

namespace Shared.Types
{
  public class TSupportSettings
  {
    public TValidationResult Result
    {
      get;
      private set;
    }

    #region Constructor
    TSupportSettings ()
    {
      Result = TValidationResult.CreateDefault;
      IniFileManager = TIniFileManager.CreatDefault;

      m_Keys = new Dictionary<string, string>
      {
        { "ColumnWidth", "0" }
      };
    }

    TSupportSettings (string filePath, string fileName)
      : this ()
    {
      IniFileManager.SelectPath (filePath, fileName);
    }
    #endregion

    #region Members
    public bool Validate ()
    {
      bool res = false;

      Result.CopyFrom (IniFileManager.ValidatePath ());

      if (Result.IsValid) {
        if (IniFileManager.ContainsSection (SupportSection).IsFalse ()) {
          var token = IniFileManager.AddSection (SupportSection);

          foreach (var key in m_Keys) {
            IniFileManager.AddKey (token, key.Key, key.Value);
          }

          IniFileManager.SaveChanges ();
        }

        res = true;
      }

      return (res);
    }

    public void Change (string keyName, string keyValue)
    {
      if (Result.IsValid) {
        IniFileManager.ChangeKey (SupportSection, keyName, keyValue);
        IniFileManager.SaveChanges ();
      }
    }
    #endregion

    #region Property
    TIniFileManager IniFileManager
    {
      get;
    }
    #endregion

    #region Fields
    const string                                      SupportSection = "SupportSection";
    readonly Dictionary<string, string>               m_Keys;
    #endregion

    #region Static
    public static TSupportSettings Create (string filePath, string fileName) => new TSupportSettings (filePath, fileName); 
    #endregion
  };
  //---------------------------//

}  // namespace