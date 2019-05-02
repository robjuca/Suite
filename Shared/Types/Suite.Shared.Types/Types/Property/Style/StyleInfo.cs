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
    public TContentStyle.Mode StyleMode
    {
      get;
      private set;
    }

    public TContentStyle.Style Style
    {
      get;
      private set;
    }

    public string StyleModeString
    {
      get
      {
        return (StyleMode.ToString ());
      }
    }

    public string StyleString
    {
      get
      {
        return (Style.ToString ());
      }
    }

    public string StyleFullString
    {
      get
      {
        var contentStyle = TContentStyle.CreateDefault;

        return (
          $"{StyleString} ({contentStyle.RequestStyleSizeString (StyleMode, Style)})"
        );
      }
    }

    public bool IsValidated
    {
      get
      {
        return (Style.Equals (TContentStyle.Style.None).IsFalse ());
      }
    }

    public bool IsStyleModeHorizontal
    {
      get
      {
        return (StyleMode.Equals (TContentStyle.Mode.Horizontal));
      }
    }

    public bool IsStyleModeVertical
    {
      get
      {
        return (StyleMode.Equals (TContentStyle.Mode.Vertical));
      }
    }
    #endregion

    #region Constructor
    TStyleInfo ()
    {
      StyleMode = TContentStyle.Mode.None;
      Style = TContentStyle.Style.None;
    }

    TStyleInfo (TContentStyle.Mode styleMode)
      : this ()
    {
      StyleMode = styleMode;
      Style = TContentStyle.Style.mini;
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

    public void CopyFrom (TStyleInfo alias)
    {
      if (alias.NotNull ()) {
        StyleMode = alias.StyleMode;
        Style = alias.Style;
      }
    }
    #endregion

    #region Static
    public static TStyleInfo CreateDefault => new TStyleInfo ();

    public static TStyleInfo Create (TContentStyle.Mode styleMode) => new TStyleInfo (styleMode);
    #endregion
  };
  //---------------------------//

}  // namespace
