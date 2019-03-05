/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public class TStyleInfo
  {
    #region Property
    public TStyleLayout Layout
    {
      get;
      private set;
    }

    public TContentStyle.Style Style
    {
      get;
      private set;
    }

    public string LayoutString
    {
      get
      {
        return (Layout.ToString ());
      }
    }

    public string StyleString
    {
      get
      {
        return (Style.ToString ());
      }
    }

    public bool IsValidated
    {
      get
      {
        return (Style.Equals (TContentStyle.Style.None).IsFalse ());
      }
    }

    public bool IsLayoutHorizontal
    {
      get
      {
        return (Layout.Equals (TStyleLayout.Horizontal));
      }
    }

    public bool IsLayoutVertical
    {
      get
      {
        return (Layout.Equals (TStyleLayout.Vertical));
      }
    }
    #endregion

    #region Constructor
    TStyleInfo ()
    {
      Layout = TStyleLayout.None;
      Style = TContentStyle.Style.None;
    }

    TStyleInfo (TStyleLayout layout)
      : this ()
    {
      Layout = layout;
    }
    #endregion

    #region Members
    public void Select (TContentStyle.Style style)
    {
      Style = style;
    }

    public void Select (string styleString)
    {
      var style = TContentStyle.TryToParse (styleString);

      Select (style);
    }

    public bool Contains (TStyleInfo styleInfo)
    {
      return (Style.Equals (styleInfo.Style));
    }
    #endregion

    #region Static
    public static TStyleInfo CreateDefault => new TStyleInfo ();

    public static TStyleInfo Create (TStyleLayout layout) => new TStyleInfo (layout);
    #endregion
  };
  //---------------------------//

}  // namespace
