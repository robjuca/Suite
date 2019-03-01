/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Message
{
  //----- TNavigateRequestMessage
  public sealed class TNavigateRequestMessage : TNavigateMessage
  {
    #region Constructor
    public TNavigateRequestMessage (TWhere where)
      : base (TAction.Request, TSender.None, where)
    {
    }

    public TNavigateRequestMessage (TSender sender, TWhere where)
      : base (TAction.Request, sender, where)
    {
    }
    #endregion
  };
  //---------------------------//

  //----- TNavigateResponseMessage
  public sealed class TNavigateResponseMessage : TNavigateMessage
  {
    #region Constructor
    public TNavigateResponseMessage (TSender sender, TWhere where, Type typeNavigateTo)
      : base (TAction.NavigateTo, sender, where, typeNavigateTo)
    {
    }
    #endregion
  };
  //---------------------------//

  //----- TModuleNavigateRequestMessage
  public sealed class TModuleNavigateRequestMessage : TNavigateMessage
  {
    #region Constructor
    public TModuleNavigateRequestMessage (TWhere where)
      : base (TAction.Request, TSender.None, where)
    {
    }

    public TModuleNavigateRequestMessage (TSender sender, TWhere where)
      : base (TAction.Request, sender, where)
    {
    }
    #endregion
  };
  //---------------------------//

  //----- TModuleNavigateResponseMessage
  public sealed class TModuleNavigateResponseMessage : TNavigateMessage
  {
    #region Constructor
    public TModuleNavigateResponseMessage (TSender sender, TWhere where, Type typeNavigateTo)
      : base (TAction.NavigateTo, sender, where, typeNavigateTo)
    {
    }
    #endregion
  };
  //---------------------------//


}  // namespace
