﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
//---------------------------//

namespace Shared.Layout.Chest
{
  public sealed class TComponentDesignControl : TComponentControlBase
  {
    #region Constructor
    public TComponentDesignControl ()
      : base ()
    {
      MyType = TType.Design;

      VerticalAlignment = VerticalAlignment.Center;
    }
    #endregion
  };
  //---------------------------//

}  // namespace