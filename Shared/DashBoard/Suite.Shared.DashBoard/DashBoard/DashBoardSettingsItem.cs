/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Types;

using Shared.Types;
//---------------------------//

namespace Shared.DashBoard
{
  public class TDashBoardSettingsItem
  {
    #region Property
    public TPosition Position
    {
      get;
      private set;
    }

    public TStyleInfo HorizontalStyleInfo
    {
      get;
    }

    public TStyleInfo VerticalStyleInfo
    {
      get;
    }

    public string SettingsString
    {
      get
      {
        var settings = string.Empty;
        var contentStyle = TContentStyle.CreateDefault;

        // mini, mini
        if (ContainsColumnPosition (1) && ContainsRowPosition (1)) {
          settings = $"{contentStyle.RequestStyleSizeString (TContentStyle.Mode.Horizontal, TContentStyle.Style.mini)} x {contentStyle.RequestStyleSizeString (TContentStyle.Mode.Vertical, TContentStyle.Style.mini)}";
        }

        // big, big
        if (ContainsColumnPosition (4) && ContainsRowPosition (4)) {
          settings = contentStyle.WindowSizeString;
        }

        return (settings);
      }
    }

    public string StyleString
    {
      get
      {
        return ($"({HorizontalStyleInfo.StyleFullString}, {VerticalStyleInfo.StyleFullString})");
      }
    }

    public string StringPosition
    {
      get
      {
        return ($"c{Position.Column} r{Position.Row}");
      }
    }
    #endregion

    #region Constructor
    TDashBoardSettingsItem (TPosition position)
      : this ()
    {
      Position.CopyFrom (position);

      switch (Position.Column) {
        case 1:
          HorizontalStyleInfo.Select (TContentStyle.Style.mini);
          break;

        case 2:
          HorizontalStyleInfo.Select (TContentStyle.Style.small);
          break;

        case 3:
          HorizontalStyleInfo.Select (TContentStyle.Style.large);
          break;

        case 4:
          HorizontalStyleInfo.Select (TContentStyle.Style.big);
          break;
      }

      switch (Position.Row) {
        case 1:
          VerticalStyleInfo.Select (TContentStyle.Style.mini);
          break;

        case 2:
          VerticalStyleInfo.Select (TContentStyle.Style.small);
          break;

        case 3:
          VerticalStyleInfo.Select (TContentStyle.Style.large);
          break;

        case 4:
          VerticalStyleInfo.Select (TContentStyle.Style.big);
          break;
      }
    }

    TDashBoardSettingsItem ()
    {
      HorizontalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      VerticalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Vertical);
      Position = TPosition.CreateDefault;
    }
    #endregion

    #region Members
    public bool IsPosition (TPosition position)
    {
      return (Position.IsPosition (position));
    }

    public bool ContainsColumnPosition (int col)
    {
      return (Position.Column.Equals (col));
    }

    public bool ContainsRowPosition (int col)
    {
      return (Position.Row.Equals (col));
    }

    public bool ContainsStyle (TContentStyle.Style horizontalStyle, TContentStyle.Style verticalStyle)
    {
      return (HorizontalStyleInfo.Style.Equals (horizontalStyle) && VerticalStyleInfo.Style.Equals (verticalStyle));
    }
    #endregion

    #region Static
    public static TDashBoardSettingsItem Create (TPosition position) => new TDashBoardSettingsItem (position); 
    #endregion
  }
  //---------------------------//

}  // namespace
