/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ComponentRelation
  {
    #region Constructor
    public ComponentRelation ()
    {
      ChildId = Guid.Empty;
      ParentId = Guid.Empty;
      ChildCategory = 0;
      ParentCategory = 0;
      Locked = false;
      PositionColumn = 0;
      PositionRow = 0;
      PositionIndex = -1;
    }

    public ComponentRelation (ComponentRelation alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ComponentRelation alias)
    {
      if (alias.NotNull ()) {
        ChildId = alias.ChildId;
        ParentId = alias.ParentId;
        ChildCategory = alias.ChildCategory;
        ParentCategory = alias.ParentCategory;
        Locked = alias.Locked;
        PositionColumn = alias.PositionColumn;
        PositionRow = alias.PositionRow;
        PositionIndex = alias.PositionIndex;
      }
    }

    public void Change (ComponentRelation alias)
    {
      if (alias.NotNull ()) {
        ParentId = alias.ParentId;
        ChildCategory = alias.ChildCategory;
        ParentCategory = alias.ParentCategory;
        Locked = alias.Locked;
        PositionColumn = alias.PositionColumn;
        PositionRow = alias.PositionRow;
        PositionIndex = alias.PositionIndex;
      }
    }
    #endregion

    #region Static
    public static ComponentRelation CreateDefault => (new ComponentRelation ());
    #endregion
  };
  //---------------------------//

}  // namespace