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
      set;
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
        var width = contentStyle.MiniSize.Width;
        var height = contentStyle.MiniSize.Height;

        // columns
        if (styleHorizontalInfo.IsLayoutHorizontal) {
          switch (styleHorizontalInfo.Style) {
            case TContentStyle.Style.small:
              width = contentStyle.SmallSize.Width;
              break;

            case TContentStyle.Style.large:
              width = contentStyle.LargeSize.Width;
              break;

            case TContentStyle.Style.big:
              width = contentStyle.LargeSize.Width;
              break;
          }
        }

        // rows
        if (styleVerticalInfo.IsLayoutVertical) {
          switch (styleVerticalInfo.Style) {
            case TContentStyle.Style.small:
              height = contentStyle.SmallSize.Height;
              break;

            case TContentStyle.Style.large:
              height = contentStyle.LargeSize.Height;
              break;

            case TContentStyle.Style.big:
              height = contentStyle.BigSize.Height;
              break;
          }
        }

        switch (Position) {
          case TImagePosition.Left:
          case TImagePosition.Right:
            Size.Width = (int) (width * .5); // 50%
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