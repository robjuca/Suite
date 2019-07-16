/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionImage
  {
    #region Constructor
    public ExtensionImage ()
    {
      Id = Guid.Empty;
      Image = null;
      Distorted = false;
      Width = 0;
      Height = 0;
      IsCommit = true;
    }

    public ExtensionImage (ExtensionImage alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionImage alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Image = alias.Image;
        Distorted = alias.Distorted;
        Width = alias.Width;
        Height = alias.Height;
        IsCommit = alias.IsCommit;
      }
    }

    public void Change (ExtensionImage alias)
    {
      if (alias.NotNull ()) {
        Image = alias.Image;
        Distorted = alias.Distorted;
        Width = alias.Width;
        Height = alias.Height;
        IsCommit = alias.IsCommit;
      }
    }

    public void ClearCommit ()
    {
      IsCommit = false;
    }
    #endregion

    #region Static
    public static ExtensionImage CreateDefault => (new ExtensionImage ());
    #endregion
  };
  //---------------------------//

}  // namespace