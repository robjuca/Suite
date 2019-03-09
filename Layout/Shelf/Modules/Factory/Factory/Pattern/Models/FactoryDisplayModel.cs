/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.Types;

using Shared.Module.Shelf;
//---------------------------//

namespace Module.Factory.Pattern.Models
{
  public class TFactoryDisplayModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      private set;
    }

    public TContentInfo ContentInfo
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TFactoryDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
      ContentInfo = TContentInfo.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (TContentInfo contentInfo)
    {
      contentInfo.ThrowNull ();

      ContentInfo.CopyFrom (contentInfo);
    }

    internal void ChangeSize (TSize size)
    {
      ComponentControlModel.ChangeSize (size);
    } 
    #endregion
  };
  //---------------------------//

}  // namespace