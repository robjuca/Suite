/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ComponentDescriptor
  {
    #region Constructor
    public ComponentDescriptor ()
    {
      Id = Guid.Empty;
      Category = 0;
    }

    public ComponentDescriptor (ComponentDescriptor alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ComponentDescriptor alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Category = alias.Category;
      }
    }

    public void Change (ComponentDescriptor alias)
    {
      if (alias.NotNull ()) {
        Category = alias.Category;
      }
    }
    #endregion

    #region Static
    public static ComponentDescriptor CreateDefault => (new ComponentDescriptor ());
    #endregion
  };
  //---------------------------//

}  // namespace