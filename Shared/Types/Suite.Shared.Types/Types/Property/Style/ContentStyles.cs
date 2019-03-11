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
  public class TContentStyle
  {
    #region Data
    public enum Style
    {
      mini,
      small,
      large,
      big,
      None,
    };

    public enum Mode
    {
      Horizontal,
      Vertical,
      None,
    };
    #endregion

    #region Property
    public TSize MiniSize
    {
      get;
      private set;
    }

    public TSize SmallSize
    {
      get
      {
        var size = TSize.CreateDefault;
        size.Width = MiniSize.Width;
        size.Height = (MiniSize.Height * 2);

        return (size); // 1c x 2r
      }
    }

    public TSize LargeSize
    {
      get
      {
        var size = TSize.CreateDefault;
        size.Width = MiniSize.Width;
        size.Height = (MiniSize.Height * 3);

        return (size); // 1c x 3r
      }
    }

    public TSize BigSize
    {
      get
      {
        var size = TSize.CreateDefault;
        size.Width = MiniSize.Width;
        size.Height = (MiniSize.Height * 4);

        return (size); // 1c x 4r
      }
    }
    #endregion

    #region Static Property
    public static string MINI => (Style.mini.ToString ());

    public static string SMALL => (Style.small.ToString ());

    public static string LARGE => (Style.large.ToString ());

    public static string BIG => (Style.big.ToString ());

    public static string NONE => (Style.None.ToString ());

    public static Array GetValues => (Enum.GetValues (typeof (Style)));

    public static string [] Names = new string [] { MINI, SMALL, LARGE, BIG, NONE };

    public static int ColumnWidth
    {
      get
      {
        return (m_ColumnWidth);
      }
    }
    #endregion

    #region Constructor
    TContentStyle ()
    {
      if (m_ColumnWidth.Equals (0)) {
        SelectColumnWidth (300);
      }
    }
    #endregion

    #region Members
    public void SelectColumnWidth (int colunWidth)
    {
      m_ColumnWidth = colunWidth;
      
      var miniHeight = (int) (colunWidth * .5); // 50%

      MiniSize = TSize.CreateDefault;
      MiniSize.Width = colunWidth;
      MiniSize.Height = miniHeight;
    }

    public static Style TryToParse (string style)
    {
      Style someStyle = Style.None;

      if (string.IsNullOrEmpty (style).IsFalse ()) {
        Enum.TryParse (style.Trim (), out someStyle);
      }

      return (someStyle);
    }
    #endregion

    #region Fields
    static int                              m_ColumnWidth = 0;
    #endregion

    #region Static
    public static TContentStyle CreateDefault => new TContentStyle (); 
    #endregion
  };
  //---------------------------//

}  // namespace
