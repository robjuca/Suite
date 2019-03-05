/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;
//---------------------------//

namespace Shared.Types
{
  public class TStylePropertyItem
  {
    #region Property
    public TStyleInfo StyleInfo
    {
      get; 
    }

    public TSize Size
    {
      get;
      private set;
    }

    public string SizeString
    {
      get
      {
        return ($"{Size.Width} x {Size.Height}");
      }
    }
    #endregion

    #region Constructor
    TStylePropertyItem ()
    {
      StyleInfo = TStyleInfo.CreateDefault;

      Size = TSize.CreateDefault;
    }

    public TStylePropertyItem (TStyleLayout layout, TContentStyle.Style style)
      : this ()
    {
      StyleInfo = TStyleInfo.Create (layout);
      StyleInfo.Select (style);
    }
    #endregion

    #region Static
    public static TStylePropertyItem CreateDefault => new TStylePropertyItem (); 
    #endregion
  };
  //---------------------------//

}  // namespace