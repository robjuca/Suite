/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Types;
using rr.Library.Helper;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Shared.DashBoard
{
  public class TDashBoardItem
  {
    #region Data
    public enum TDashBoardStatus
    {
      Busy,
      Disable,
      Standby,
    };
    #endregion

    #region Property
    public TDashBoardStatus DahBoardStatus
    {
      get;
      private set;
    }

    public TComponentItemInfo ComponentItemInfo
    {
      get;
      private set;
    }

    public TPosition Position
    {
      get;
      private set;
    }

    public TSize Size
    {
      get
      {
        return (ComponentItemInfo.Model.Size);
      }
    }

    public bool IsStandby
    {
      get
      {
        return (DahBoardStatus.Equals (TDashBoardStatus.Standby));
      }
    }

    public bool IsBusy
    {
      get
      {
        return (DahBoardStatus.Equals (TDashBoardStatus.Busy));
      }
    }

    public bool IsDisable
    {
      get
      {
        return (DahBoardStatus.Equals (TDashBoardStatus.Disable));
      }
    }

    public bool IsRoot
    {
      get
      {
        return (Id.IsEmpty ().IsFalse ());
      }
    }

    public Guid Id
    {
      get;
      private set;
    }

    public string Name
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

    public string Background
    {
      get;
      private set;
    }

    public DataTemplate IconTemplate
    {
      get
      {
        return (TDataTemplateXaml.RequestTemplate (IconXmlTemplate));
      }
    }

    public string IconXmlTemplate
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get
      {
        return (ComponentItemInfo.Category);
      }
    }

    public int ChildCategory
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TDashBoardItem (TPosition position)
      : this ()
    {
      Position.CopyFrom (position);
    }

    public TDashBoardItem (TDashBoardItem alias)
      : this ()
    {
      CopyFrom (alias);
    }

    TDashBoardItem ()
    {
      HorizontalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      VerticalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Vertical);
      Position = TPosition.CreateDefault;

      Cleanup ();
    }
    #endregion

    #region Members
    public bool ContainsPoint (int x, int y)
    {
      return (IsStandby ? MyRectangle.Contains (x, y) : false);
    }

    public bool IsPosition (TDashBoardItem alias)
    {
      return (IsPosition (alias.Position));
    }

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

    public bool CanDropBySize (TDashBoardItem alias)
    {
      if (IsStandby) {
        if (IsSize (alias)) {
          return (true);
        }
      }

      return (IsSize (alias));
    }

    public bool IsSize (TDashBoardItem alias)
    {
      return (Size.IsSize (alias.Size));
    }

    public void ChangeStatus (TDashBoardStatus status)
    {
      DahBoardStatus = status;

      if (status.Equals (TDashBoardStatus.Standby)) {
        Cleanup ();
      }
    }

    public void DisableStatus ()
    {
      DahBoardStatus = TDashBoardStatus.Disable;
      SelectBackground ();
    }

    public void ChangeName (string name)
    {
      if (string.IsNullOrEmpty (name).IsFalse ()) {
        Name = name;
      }
    }

    public void SelectModel (TComponentModelItem modelItem)
    {
      if (modelItem.NotNull ()) {
        ComponentItemInfo.Model.CopyFrom (modelItem);

        Name = ComponentItemInfo.Model.Name;
        Id = ComponentItemInfo.Model.Id;
        HorizontalStyleInfo.Select (modelItem.LayoutModel.StyleHorizontal);
        VerticalStyleInfo.Select (modelItem.LayoutModel.StyleVertical);

        if (modelItem.NodeModelCollection.Count.Equals (1)) {
          var node = modelItem.NodeModelCollection [0];
          ChildCategory = node.ChildCategory;
        }
      }

      SelectIconResource ();
    }

    public TComponentModelItem RequestModel ()
    {
      var model = TComponentModelItem.CreateDefault;
      model.CopyFrom (ComponentItemInfo.Model);

      return (model);
    }

    public bool DisableByColumn (int column)
    {
      if (ContainsColumnPosition (column)) {
        DisableStatus ();
        return (true);
      }

      return (false);
    }

    public bool DisableByRow (int row)
    {
      if (ContainsRowPosition (row)) {
        DisableStatus ();
        return (true);
      }

      return (false);
    }

    public void CopyFrom (TDashBoardItem alias, bool preservePosition = false)
    {
      if (alias.NotNull ()) {
        DahBoardStatus = alias.DahBoardStatus;

        if (preservePosition.IsFalse ()) {
          Position.CopyFrom (alias.Position);
        }

        Name = alias.Name;
        Id = alias.Id;

        HorizontalStyleInfo.CopyFrom (alias.HorizontalStyleInfo);
        VerticalStyleInfo.CopyFrom (alias.VerticalStyleInfo);

        ComponentItemInfo.Model.CopyFrom (alias.ComponentItemInfo.Model);
        ChildCategory = alias.ChildCategory;

        IconXmlTemplate = alias.IconXmlTemplate;

        SelectBackground (alias.Background);

        SelectIconResource ();
      }
    }

    public bool Reset (out TPosition position, Guid id)
    {
      position = TPosition.CreateDefault;

      if (Id.Equals (id)) {
        position.CopyFrom (Position);

        Cleanup ();

        return (true);
      }

      return (false);
    }

    public void RequestBackground ()
    {
      Background = TRandomColors.ColorToHtml ();
      Background = Background.Replace ("Dark", "");
      Background = Background.Replace ("White", "Red");
      Background = Background.Replace ("Black", "Green");
    }

    public void SelectBackground (string background)
    {
      Background = background;
    }

    public void SelectIconResource ()
    {
      IconXmlTemplate = "<ContentControl Style='{DynamicResource ";
      IconXmlTemplate += $"Content{Category}Icon";
      IconXmlTemplate += "}' />";
    }

    public bool IsSameStyle (TDashBoardItem alias)
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
      ComponentItemInfo = TComponentItemInfo.CreateDefault;

      ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.None);

      DahBoardStatus = TDashBoardStatus.Standby;

      Name = string.Empty;
      Id = Guid.Empty;
      Background = "#ffffff";

      // bag style mini (300 x 116) (margin 2)
      var width = 304;
      var height = 120;

      MyRectangle = new System.Drawing.Rectangle (
        width * (Position.Column - 1),
        height * (Position.Row - 1),
        width,
        height
      );

      SelectIconResource ();
    }
    #endregion

    #region Property
    System.Drawing.Rectangle MyRectangle
    {
      get;
      set;
    }
    #endregion

    #region Support
    void SelectBackground ()
    {
      Background = "#ffffff";

      switch (DahBoardStatus) {
        case TDashBoardStatus.Disable: {
            Background = "#ddd1cc";
          }
          break;
      }
    }
    #endregion

    #region Static
    public static TDashBoardItem CreateDefault => new TDashBoardItem (); 
    #endregion
  }
  //---------------------------//

}  // namespace
