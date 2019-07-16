/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ComponentStatus
  {
    #region Constructor
    public ComponentStatus ()
    {
      Id = Guid.Empty;

      Locked = false;
      Busy = false;
      Active = false;
    }

    public ComponentStatus (ComponentStatus alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ComponentStatus alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;

        Locked = alias.Locked;
        Busy = alias.Busy;
        Active = alias.Active;
      }
    }

    public void Change (ComponentStatus alias)
    {
      if (alias.NotNull ()) {
        Locked = alias.Locked;
        Busy = alias.Busy;
        Active = alias.Active;
      }
    }
    #endregion

    #region Static
    public static ComponentStatus CreateDefault => (new ComponentStatus ());
    #endregion
  };
  //---------------------------//

}  // namespace