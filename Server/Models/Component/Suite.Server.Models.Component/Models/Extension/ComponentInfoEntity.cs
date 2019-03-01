/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ComponentInfo
  {
    #region Constructor
    public ComponentInfo ()
    {
      Id = Guid.Empty;
      Name = string.Empty;
      Enabled = false;
    }

    public ComponentInfo (ComponentInfo alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ComponentInfo alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Name = alias.Name;
        Enabled = alias.Enabled;
      }
    }

    public void Change (ComponentInfo alias)
    {
      if (alias.NotNull ()) {
        Name = alias.Name;
        Enabled = alias.Enabled;
      }
    }
    #endregion

    #region Static
    public static ComponentInfo CreateDefault => (new ComponentInfo ());
    #endregion
  };
  //---------------------------//

}  // namespace