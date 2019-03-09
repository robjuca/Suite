/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Types;
//---------------------------//

namespace Shared.Gadget.Document
{
  public sealed class TComponentDesignControl : TComponentControlBase
  {
    #region Constructor
    public TComponentDesignControl ()
      : base (TControlMode.Design)
    {
    }
    #endregion
  };
  //---------------------------//

}  // namespace