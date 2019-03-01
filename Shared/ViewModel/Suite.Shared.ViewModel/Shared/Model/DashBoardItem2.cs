/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using rr.Library.Types;
//---------------------------//

namespace Shared.Module.Drawer
{
  public class TDashBoardItem
  {
    #region Property
    public TContentType ContentType
    {
      get;
      private set;
    }

    public TPosition ItemPosition
    {
      get;
      private set;
    }

    public TSize ItemSize
    {
      get
      {
        return (ModelInfo.ModelSize);
      }
    }

    public TDashBoardStatus DahBoardStatus
    {
      get;
      private set;
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

    public bool IsIdValidate
    {
      get
      {
        return (ContextId.NotEmpty ());
      }
    }

    public bool IsRoot
    {
      get
      {
        return (IsBusy && IsIdValidate);
      }
    }

    public bool IsDisable
    {
      get
      {
        return (DahBoardStatus.Equals (TDashBoardStatus.Disable));
      }
    }

    public Guid ContextId
    {
      get;
      private set;
    }

    public string Name
    {
      get;
      private set;
    }

    public string Position
    {
      get
      {
        return ($"c{ItemPosition.Column} r{ItemPosition.Row}");
      }
    }

    public string Background
    {
      get;
      private set;
    }

    public Visibility ContentVisibility
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    TDashBoardItem (TPosition position)
      : this ()
    {
      if (position.NotNull ()) {
        ItemPosition.CopyFrom (position);
      }
    }

    TDashBoardItem ()
    {
      ItemPosition = TPosition.CreateDefault;

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
      return (alias.IsNull () ? false : ItemPosition.IsPosition (alias.ItemPosition));
    }

    public bool IsPosition (TPosition position)
    {
      return (position.IsNull () ? false : ItemPosition.IsPosition (position));
    }

    public bool IsSameSize (TDashBoardItem alias)
    {
      return (alias.IsNull () ? false : ModelInfo.IsSameSize (alias.ModelInfo));
    }

    public bool ContainsColumnPosition (int col)
    {
      return (ItemPosition.Column.Equals (col));
    }

    public bool ContainsRowPosition (int col)
    {
      return (ItemPosition.Row.Equals (col));
    }

    public void ChangeStatus (TDashBoardStatus status, TContentType contentType, string color)
    {
      DahBoardStatus = status;

      ChangeContentType (contentType);
      SelectBackground (color); ;

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

    public void Update (TModelInfo model)
    {
      ChangeContentType (model.ContentType);

      Name = model.ModelName;
      ContextId = model.ModelId;

      ModelInfo.CopyFrom (model);
    }

    public TModelInfo RequestModelInfo ()
    {
      return (new TModelInfo (ModelInfo));
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

    public void CopyFrom (TDashBoardItem alias, bool copyPosition = false)
    {
      if (alias.NotNull ()) {
        ChangeContentType (alias.ContentType);

        DahBoardStatus = alias.DahBoardStatus;

        if (copyPosition) {
          ItemPosition.CopyFrom (alias.ItemPosition);
        }

        Name = alias.Name;
        ContextId = alias.ContextId;
        Background = alias.Background;

        ModelInfo.CopyFrom (alias.ModelInfo);
      }
    }

    public bool Reset (out TPosition position, Guid id)
    {
      position = TPosition.CreateDefault;

      if (ContextId.Equals (id)) {
        position.CopyFrom (ItemPosition);

        Cleanup ();

        return (true);
      }

      return (false);
    }

    public void Cleanup ()
    {
      ChangeContentType (TContentType.None);

      DahBoardStatus = TDashBoardStatus.Standby;

      Name = string.Empty;
      ContextId = Guid.Empty;
      Background = "#ffffff";

      // bag style mini (300 x 116) (margin 2)
      var width = 304;
      var height = 120;

      MyRectangle = new System.Drawing.Rectangle (
        width * (ItemPosition.Column - 1),
        height * (ItemPosition.Row - 1),
        width,
        height
      );

      ModelInfo = TModelInfo.CreateDefault;
    }
    #endregion

    #region Property
    protected TModelInfo ModelInfo
    {
      get;
      private set;
    }

    System.Drawing.Rectangle MyRectangle
    {
      get;
      set;
    }
    #endregion

    #region Static
    public static TDashBoardItem CreateAlias (TDashBoardItem alias)
    {
      var item = CreateDefault;

      if (alias.NotNull ()) {
        item.CopyFrom (alias);
      }

      return (item);
    }
    public static TDashBoardItem Create (TPosition position) => new TDashBoardItem (position);

    public static TDashBoardItem CreateDefault => new TDashBoardItem ();
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

    void SelectBackground (string color)
    {
      Background = "#ffffff";

      switch (DahBoardStatus) {
        case TDashBoardStatus.Busy: {
            Background = color;
          }
          break;

        case TDashBoardStatus.Disable: {
            Background = "#ddd1cc";
          }
          break;
      }
    }

    void ChangeContentType (TContentType contentType)
    {
      ContentType = contentType;

      UpdateVisibility ();
    }

    void UpdateVisibility ()
    {
      ContentVisibility = ContentType.Equals (TContentType.Shelf) ? Visibility.Visible : Visibility.Collapsed;
    }
    #endregion
  }
  //---------------------------//

}  // namespace
