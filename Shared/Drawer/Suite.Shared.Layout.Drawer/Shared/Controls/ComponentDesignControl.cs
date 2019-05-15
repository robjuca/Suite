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
  public sealed class TComponentDesignControl : TComponentControlBase
  {
    #region Constructor
    public TComponentDesignControl ()
      : base (TControlMode.Design)
    {
      VerticalAlignment = VerticalAlignment.Center;
    }
    #endregion
  };
  //---------------------------//

}  // namespace