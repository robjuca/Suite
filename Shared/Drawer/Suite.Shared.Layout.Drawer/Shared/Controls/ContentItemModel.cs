﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;
//---------------------------//

namespace Shared.Layout.Drawer
{
  public class TContentItemModel
  {
    #region Property
    public Shared.Layout.Shelf.TComponentControlModel ComponentControlModel
    {
      get;
      private set;
    }

    public TPosition Position
    {
      get;
      private set;
    }

    public Guid Id
    {
      get
      {
        return (ComponentControlModel.Id);
      }
    }
    #endregion

    #region Constructor
    public TContentItemModel (TPosition position, Shared.Layout.Shelf.TComponentControlModel model)
      : this ()
    {
      Position.CopyFrom (position);
      ComponentControlModel.CopyFrom (model);
    }

    TContentItemModel ()
    {
      Position = TPosition.CreateDefault;
      ComponentControlModel = Shared.Layout.Shelf.TComponentControlModel.CreateDefault;
    }
    #endregion

    #region Members
    public bool IsPosition (TPosition position)
    {
      return (position.IsNull () ? false : Position.IsPosition (position));
    }

    public bool ContainsId (Guid id)
    {
      return (Id.Equals (id));
    }

    public void ChangePosition (TPosition position)
    {
      Position.CopyFrom (position);
    }
    #endregion
  };
  //---------------------------//

}  // namespace