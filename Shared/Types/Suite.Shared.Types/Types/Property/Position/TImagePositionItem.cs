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
  public class TImagePositionItem
  {
    #region Property
    public TImagePosition Position
    {
      get;
      private set;
    }

    public string PositionString
    {
      get
      {
        return (Position.ToString ());
      }
    }

    public string SizeString
    {
      get
      {
        return ($"{Size.Width} x {Size.Height}");
      }
    }

    public TSize Size
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TImagePositionItem ()
    {
      Position = TImagePosition.None;
      Size = TSize.CreateDefault;
    }

    public TImagePositionItem (TStyleInfo styleHorizontalInfo, TStyleInfo styleVerticalInfo, TImagePosition imagePosition)
      : this ()
    {
      Position = imagePosition;

      if (imagePosition.Equals (TImagePosition.None).IsFalse ())  {
        var contentStyle = TContentStyle.CreateDefault;
        var width = contentStyle.RequestStyleSize (styleHorizontalInfo.StyleMode, styleHorizontalInfo.Style);
        var height = contentStyle.RequestStyleSize (styleVerticalInfo.StyleMode, styleVerticalInfo.Style);

        switch (Position) {
          case TImagePosition.Left:
          case TImagePosition.Right:
            Size.Width = (int) (width * .4); // 40%
            Size.Height = height;
            break;

          case TImagePosition.Top:
          case TImagePosition.Bottom:
            Size.Width = width;
            Size.Height = (int) (height * .4); // 40%
            break;

          case TImagePosition.Full:
            Size.Width = width;
            Size.Height = height;
            break;
        }
      }
    }
    #endregion

    #region Static
    public static TImagePositionItem CreateDefault => new TImagePositionItem (); 
    #endregion
  };
  //---------------------------//

}  // namespace