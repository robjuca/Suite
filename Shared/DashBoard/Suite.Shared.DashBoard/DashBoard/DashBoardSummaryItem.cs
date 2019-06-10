/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Shared.DashBoard
{
  public class TDashBoardSummaryItem
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

    public int Summary
    {
      get;
      private set;
    }

    public string SummaryString
    {
      get
      {
        return (Summary.Equals (0) ? string.Empty : $"{Summary}");
      }
    }

    public Visibility SummaryVisibility
    {
      get
      {
        return (Summary.Equals (0) ? Visibility.Hidden : Visibility.Visible);
      }
    }
    #endregion

    #region Constructor
    TDashBoardSummaryItem (TPosition position)
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

    TDashBoardSummaryItem ()
    {
      HorizontalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      VerticalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Vertical);
      Position = TPosition.CreateDefault;

      Cleanup ();
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

    public void SelectModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        var key = HorizontalStyleInfo.StyleString + VerticalStyleInfo.StyleString;

        if (action.Summary.GadgetCount.ContainsKey (key)) {
          Summary = action.Summary.GadgetCount [key];
        }
      }
    }

    public void CopyFrom (TDashBoardSummaryItem alias, bool preservePosition = false)
    {
      if (alias.NotNull ()) {
        
      }
    }

    public bool IsSameStyle (TDashBoardSummaryItem alias)
    {
      bool res = false;

      if (alias.NotNull ()) {
        res = HorizontalStyleInfo.Contains (alias.HorizontalStyleInfo) && VerticalStyleInfo.Contains (alias.VerticalStyleInfo);
      }

      return (res);
    }

    public bool ContainsStyle (TContentStyle.Style horizontalStyle, TContentStyle.Style verticalStyle)
    {
      return (HorizontalStyleInfo.Style.Equals (horizontalStyle) && VerticalStyleInfo.Style.Equals (verticalStyle));
    }

    public void Cleanup ()
    {
      // style mini (300 x 116) (margin 2)
      var width = 304;
      var height = 120;

      MyRectangle = new System.Drawing.Rectangle (
        width * (Position.Column - 1),
        height * (Position.Row - 1),
        width,
        height
      );

      Summary = 0;
    }
    #endregion

    #region Property
    System.Drawing.Rectangle MyRectangle
    {
      get;
      set;
    }
    #endregion

    #region Static
    public static TDashBoardSummaryItem Create (TPosition position) => new TDashBoardSummaryItem (position); 
    #endregion
  }
  //---------------------------//

}  // namespace
