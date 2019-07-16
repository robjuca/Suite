/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionLayout
  {
    #region Constructor
    public ExtensionLayout ()
    {
      Id = Guid.Empty;
      StyleHorizontal = string.Empty;
      StyleVertical = string.Empty;
      Width = 0;
      Height = 0;
    }

    public ExtensionLayout (ExtensionLayout alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionLayout alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        StyleHorizontal = alias.StyleHorizontal;
        StyleVertical = alias.StyleVertical;
        Width = alias.Width;
        Height = alias.Height;
      }
    }

    public void Change (ExtensionLayout alias)
    {
      if (alias.NotNull ()) {
        StyleHorizontal = alias.StyleHorizontal;
        StyleVertical = alias.StyleVertical;
        Width = alias.Width;
        Height = alias.Height;
      }
    }
    #endregion

    #region Static
    public static ExtensionLayout CreateDefault => (new ExtensionLayout ());
    #endregion
  };
  //---------------------------//

}  // namespace