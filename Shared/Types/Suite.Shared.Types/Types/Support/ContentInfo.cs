/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public sealed class TContentInfo
  {
    #region Property
    public Guid Id
    {
      get;
      private set;
    }

    public TPosition Position
    {
      get;
      private set;
    }

    public string Style
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TContentInfo (Guid contentId, TPosition contentPosition)
      : this ()
    {
      Select (contentId, contentPosition);
    }

    TContentInfo ()
    {
      Id = Guid.Empty;
      Style = string.Empty;
      Position = TPosition.CreateDefault;
    }
    #endregion

    #region Members
    public void Select (Guid contentId, TPosition contentPosition)
    {
      Id = contentId;

      Position.CopyFrom (contentPosition);
    }

    public void Select (string style)
    {
      Style = style;
    }

    public void CopyFrom (TContentInfo alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        Style = alias.Style;
        Position.CopyFrom (alias.Position);
      }
    }
    #endregion

    #region Static
    public static TContentInfo CreateDefault => new TContentInfo (); 
    #endregion
  }
  //---------------------------//

}  // namespace