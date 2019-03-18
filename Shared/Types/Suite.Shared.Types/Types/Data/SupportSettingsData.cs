/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.Generic;
//---------------------------//

namespace Shared.Types
{
  public sealed class TSupportSettingsData
  {
    #region Property
    public List<string> SettingsNames
    {
      get;
    } 
    #endregion

    #region Constructor
    TSupportSettingsData ()
    {
      SettingsNames = new List<string> ()
      {
        "ColumnWidth"
      };

      if (m_Settings.Count.Equals (0)) {
        foreach (var name in SettingsNames) {
          m_Settings.Add (name, "0");
        }
      }
    }
    #endregion

    #region Members
    public void Select (string settingsName, string settingsValue)
    {
      if (m_Settings.ContainsKey (settingsName)) {
        m_Settings [settingsName] = settingsValue;
      }
    }

    public string Request (string settingsName)
    {
      return (m_Settings.ContainsKey (settingsName) ? m_Settings [settingsName] : string.Empty);
    }
    #endregion

    #region Fields
    static readonly Dictionary<string, string>                  m_Settings = new Dictionary<string, string> (); 
    #endregion

    #region Static
    public static TSupportSettingsData CreateDefault => new TSupportSettingsData ();
    #endregion
  };
  //---------------------------//

}  // namespace