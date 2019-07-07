/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Communication
{
  [Serializable]
  public sealed class TDataComm
  {
    #region Property
    public TCommandComm Command
    {
      get;
      private set;
    }

    public string ClientName
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TDataComm ()
    {
      Command = TCommandComm.None;
      ClientName = string.Empty;
    }
    #endregion

    #region Members
    public void Select (TCommandComm command, string clientName)
    {
      Command = command;
      ClientName = clientName;
    } 
    #endregion

    #region Static
    public static TDataComm CreateDefault => new TDataComm (); 
    #endregion
  };
  //---------------------------//

}  // namespace