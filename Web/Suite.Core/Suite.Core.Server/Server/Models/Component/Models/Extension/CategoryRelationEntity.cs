/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class CategoryRelation
  {
    #region Constructor
    public CategoryRelation ()
    {
      Category = 0;
      Extension = 0;
    }

    public CategoryRelation (CategoryRelation alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (CategoryRelation alias)
    {
      if (alias.NotNull ()) {
        Category = alias.Category;
        Extension = alias.Extension;
      }
    }

    public void Change (CategoryRelation alias)
    {
      if (alias.NotNull ()) {
        Extension = alias.Extension;
      }
    }
    #endregion

    #region Static
    public static CategoryRelation CreateDefault => (new CategoryRelation ());
    #endregion
  };
  //---------------------------//

}  // namespace