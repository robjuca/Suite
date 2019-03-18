/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

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

      m_SupportSettingsData = TSupportSettingsData.CreateDefault;

      m_FilePath = System.Environment.CurrentDirectory;
      m_FileName = TNames.SettingsIniFileName;

      IniFileManager.SelectPath (m_FilePath, m_FileName);
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
        // create new section
        if (IniFileManager.ContainsSection (SupportSection).IsFalse ()) {
          var token = IniFileManager.AddSection (SupportSection);

          foreach (var settingsName in m_SupportSettingsData.SettingsNames) {
            var keyName = settingsName;
            var keyValue = m_SupportSettingsData.Request (settingsName);

            IniFileManager.AddKey (token, keyName, keyValue);
          }

          IniFileManager.SaveChanges ();
        }

        // update support settings data
        else {
          foreach (var settingsName in m_SupportSettingsData.SettingsNames) {
            if (IniFileManager.ContainsKey (SupportSection, settingsName)) {
              var settingsValue = IniFileManager.RequestKey (SupportSection, settingsName);
              m_SupportSettingsData.Select (settingsName, settingsValue);
            }
          }
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

        m_SupportSettingsData.Select (keyName, keyValue);
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
    readonly TSupportSettingsData                     m_SupportSettingsData;
    readonly string                                   m_FilePath;
    readonly string                                   m_FileName;
    #endregion

    #region Static
    public static TSupportSettings CreateDefault => new TSupportSettings ();
    public static TSupportSettings Create (string filePath, string fileName) => new TSupportSettings (filePath, fileName);
    #endregion
  };
  //---------------------------//

}  // namespace