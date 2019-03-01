/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public sealed class TReportData
  {
    #region Property
    public bool Locked
    {
      get;
      private set;
    }

    public bool Unlocked
    {
      get;
      private set;
    }

    public bool Distorted
    {
      get;
      private set;
    }

    public string Message
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    TReportData ()
    {
      Clean ();
    }
    #endregion

    #region Members
    public void Select (bool locked, bool distorted)
    {
      Locked = locked;
      Distorted = distorted;
    }

    public void Select (string message)
    {
      Message = message;
    }

    public void SelectLock ()
    {
      Locked = true;
    }

    public void SelectUnlock ()
    {
      Unlocked = true;
    }

    public void CopyFrom (TReportData alias)
    {
      Clean ();

      if (alias.NotNull ()) {
        Locked = alias.Locked;
        Unlocked = alias.Unlocked;
        Distorted = alias.Distorted;
        Message = alias.Message;
      }
    }
    #endregion

    #region Property
    public static TReportData CreateDefault => (new TReportData ());
    #endregion

    #region Support
    void Clean ()
    {
      Locked = false;
      Unlocked = false;
      Distorted = false;
      Message = string.Empty;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace