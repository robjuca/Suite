/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Layout.Collection.Pattern.Models
{
  public class TCollectionListDashBoardModel
  {
    #region Property
    public Server.Models.Infrastructure.TSummary Summary
    {
      get;
    }
    #endregion

    #region Constructor
    public TCollectionListDashBoardModel ()
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
