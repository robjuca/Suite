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

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
      private set;
    }

    public TSize Size
    {
      get;
      private set;
    }

    public TStyleInfo StyleHorizontal
    {
      get;
      private set;
    }

    public TStyleInfo StyleVertical
    {
      get;
      private set;
    }

    public string StyleString
    {
      get
      {
        return ($"{StyleHorizontal.StyleFullString}, {StyleVertical.StyleFullString}");
      }
    }
    #endregion

    #region Constructor
    //public TContentInfo (Guid contentId, TPosition contentPosition, Server.Models.Infrastructure.TCategory category)
    //  : this ()
    //{
    //  Select (contentId, contentPosition, category);
    //}

    TContentInfo ()
    {
      Id = Guid.Empty;
      StyleHorizontal = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      StyleVertical = TStyleInfo.Create (TContentStyle.Mode.Vertical);
      Position = TPosition.CreateDefault;
      Size = TSize.CreateDefault;
      Category = Server.Models.Infrastructure.TCategory.None;
    }
    #endregion

    #region Members
    public void Select (Guid contentId, TPosition contentPosition)
    {
      Id = contentId;
      Position.CopyFrom (contentPosition);
    }

    public void Select (Server.Models.Infrastructure.TCategory category)
    {
      Category = category;
    }

    public void Select (TContentStyle.Mode styleMode, string styleString)
    {
      switch (styleMode) {
        case TContentStyle.Mode.Horizontal:
          StyleHorizontal.Select (styleString);
          break;

        case TContentStyle.Mode.Vertical:
          StyleVertical.Select (styleString);
          break;
      }

      var contentStyle = TContentStyle.CreateDefault;

      Size.SelectColumns (contentStyle.RequestBoardStyleSize (StyleHorizontal.Style));
      Size.SelectRows (contentStyle.RequestBoardStyleSize (StyleVertical.Style));
    }

    public void CopyFrom (TContentInfo alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        StyleHorizontal = alias.StyleHorizontal;
        StyleVertical = alias.StyleVertical;
        Position.CopyFrom (alias.Position);
        Size = alias.Size;
        Category = alias.Category;
      }
    }
    #endregion

    #region Static
    public static TContentInfo CreateDefault => new TContentInfo (); 
    #endregion
  }
  //---------------------------//

}  // namespace