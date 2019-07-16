/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public partial class ExtensionGeometry
  {
    #region Constructor
    public ExtensionGeometry ()
    {
      Id = Guid.Empty;

      PositionCol = 0;
      PositionRow = 0;

      SizeCols = 0;
      SizeRows = 0;

      PositionImage = string.Empty;
      PositionIndex = -1;
    }

    public ExtensionGeometry (ExtensionGeometry alias)
      : this ()
    {
      CopyFrom (alias);
    }
    #endregion

    #region Members
    public void CopyFrom (ExtensionGeometry alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        PositionCol = alias.PositionCol;
        PositionRow = alias.PositionRow;
        SizeCols = alias.SizeCols;
        SizeRows = alias.SizeRows;
        PositionImage = alias.PositionImage;
        PositionIndex = alias.PositionIndex;
      }
    }

    public void Change (ExtensionGeometry alias)
    {
      if (alias.NotNull ()) {
        PositionCol = alias.PositionCol;
        PositionRow = alias.PositionRow;
        SizeCols = alias.SizeCols;
        SizeRows = alias.SizeRows;
        PositionImage = alias.PositionImage;
        PositionIndex = alias.PositionIndex;
      }
    }
    #endregion

    #region Static
    public static ExtensionGeometry CreateDefault => (new ExtensionGeometry ());
    #endregion
  };
  //---------------------------//

}  // namespace