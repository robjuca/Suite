/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public static class TContentStyle
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
    
    public const int MiniWidth = 300;
    public const int MiniHeight = 116;

    public const int SmallWidth = 300;
    public const int SmallHeight = 232;

    public const int LargeWidth = 300;
    public const int LargeHeight = 348;

    public const int BigWidth = 600;
    public const int BigHeight = 348;
    #endregion

    #region Property
    public static string MINI => (Style.mini.ToString ());

    public static string SMALL => (Style.small.ToString ());

    public static string LARGE => (Style.large.ToString ());

    public static string BIG => (Style.big.ToString ());

    public static string NONE => (Style.None.ToString ());

    public static Array GetValues => (Enum.GetValues (typeof (Style)));
    #endregion

    #region Members
    public static Style TryToParse (string style)
    {
      Style someStyle = Style.None;

      if (string.IsNullOrEmpty (style).IsFalse ()) {
        Enum.TryParse (style.Trim (), out someStyle);
      }

      return (someStyle);
    } 
    #endregion

    #region Public Field
    public static string [] Names = new string [] { MINI, SMALL, LARGE, BIG, NONE }; 
    #endregion
  };
  //---------------------------//

}  // namespace
