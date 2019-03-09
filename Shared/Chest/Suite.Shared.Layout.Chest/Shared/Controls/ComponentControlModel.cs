/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Layout.Chest
{
  public class TComponentControlModel
  {
    #region Property
    public Guid Id
    {
      get;
      set;
    }

    public Server.Models.Component.TEntityAction EntityAction
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      Id = Guid.Empty;
      EntityAction = Server.Models.Component.TEntityAction.CreateDefault;
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace