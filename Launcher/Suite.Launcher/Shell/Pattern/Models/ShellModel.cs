/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Suite.Launcher.Shell.Pattern.Models
{
  public class TShellModel
  {
    #region Property
    public bool IsMenuEnabled
    {
      get;
      set;
    }
    public bool IsSettingsEnabled
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TShellModel ()
    {
      IsMenuEnabled = false;
      IsSettingsEnabled = true;
    }
    #endregion

    #region Members
    internal void EnableAll ()
    {
      IsMenuEnabled = true;
      IsSettingsEnabled = true;
    }

    internal void DisableAll ()
    {
      IsMenuEnabled = false;
      IsSettingsEnabled = false;
    }

    internal void MenuOnly ()
    {
      IsMenuEnabled = true;
      IsSettingsEnabled = false;
    }

    internal void SettingsOnly ()
    {
      IsMenuEnabled = false;
      IsSettingsEnabled = true;
    }
    #endregion
  };
  //---------------------------//

}  // namespace