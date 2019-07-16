/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionNode
  {
    #region Constructor
    public ExtensionNode ()
    {
      ChildId = Guid.Empty;
      ParentId = Guid.Empty;

      ChildCategory = 0;
      ParentCategory = 0;

      Position = string.Empty;
      Locked = false;
    }

    public ExtensionNode (ExtensionNode alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionNode alias)
    {
      if (alias.NotNull ()) {
        ChildId = alias.ChildId;
        ParentId = alias.ParentId;

        ChildCategory = alias.ChildCategory;
        ParentCategory = alias.ParentCategory;
        Position = alias.Position;
        Locked = alias.Locked;
      }
    }

    public void Change (ExtensionNode alias)
    {
      if (alias.NotNull ()) {
        ParentId = alias.ParentId;

        ChildCategory = alias.ChildCategory;
        ParentCategory = alias.ParentCategory;
        Position = alias.Position;
        Locked = alias.Locked;
      }
    }
    #endregion

    #region Static
    public static ExtensionNode CreateDefault => (new ExtensionNode ());
    #endregion
  };
  //---------------------------//

}  // namespace