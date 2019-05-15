/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;

using Shared.Types;
//---------------------------//

namespace Shared.Layout.Drawer
{
  public sealed class TComponentDisplayControl : TComponentControlBase
  {
    #region Constructor
    public TComponentDisplayControl ()
      : base (TControlMode.Display)
    {
    }

    public TComponentDisplayControl (TComponentControlModel model)
      : base (TControlMode.Display, model)
    {
    }
    #endregion
  };
  //---------------------------//

}  // namespace