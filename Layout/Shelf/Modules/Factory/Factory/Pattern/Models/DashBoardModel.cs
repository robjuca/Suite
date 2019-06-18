/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TDashBoardModel
  {
    #region Property
    public Server.Models.Infrastructure.TSummary Summary
    {
      get;
    }
    #endregion

    #region Constructor
    public TDashBoardModel ()
    {
      Summary = Server.Models.Infrastructure.TSummary.CreateDefault;
    }
    #endregion

    #region Members
    public void Select (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      Summary.CopyFrom (action.Summary);
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
