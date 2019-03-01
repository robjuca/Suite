/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public class TConfirmMessage
  {
    #region Property
    public string Title
    {
      get;
      private set;
    }

    public string Message
    {
      get;
      private set;
    }

    public bool IsAffirmative
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TConfirmMessage ()
    {
    }
    #endregion

    #region Members
    public void SelectTitle (string title)
    {
      Title = title;
    }

    public void SelectMessage (string message)
    {
      Message = message;
    }
    #endregion
  };
  //---------------------------//

}  // namespace