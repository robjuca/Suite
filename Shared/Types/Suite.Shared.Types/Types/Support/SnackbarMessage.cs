/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Types
{
  public class TSnackbarMessage
  {
    #region Property
    public bool IsActive
    {
      get;
      set;
    }

    public string Message
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TSnackbarMessage (string message)
    {
      Message = message;
    }

    public TSnackbarMessage (TSnackbarMessage alias)
      : this ()
    {
      CopyFrom (alias);
    }

    TSnackbarMessage ()
    {
      Message = string.Empty;
    }
    #endregion

    #region Members
    public void SetMessage (string message)
    {
      Message = message;
    }

    public void Open ()
    {
      IsActive = true;
    }

    public void Close ()
    {
      Message = string.Empty;
      IsActive = false;
    }

    public void CopyFrom (TSnackbarMessage alias)
    {
      if (alias != null) {
        Message = alias.Message;
      }
    }
    #endregion

    #region Property
    public static TSnackbarMessage CreateDefault => (new TSnackbarMessage ());
    #endregion
  };
  //---------------------------//

}  // namespace