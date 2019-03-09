/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Layout.Drawer
{
  public class TComponentControlModel
  {
    #region Property
    public string Caption
    {
      get;
      set;
    }

    public int Width
    {
      get;
      set;
    }

    public int Height
    {
      get;
      set;
    }

    public Guid Id
    {
      get;
      set;
    }

    public Server.Models.Component.TEntityAction EntityAction
    {
      get;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      Caption = string.Empty;
      Width = 0;
      Height = 0;
      Id = Guid.Empty;
      EntityAction = Server.Models.Component.TEntityAction.CreateDefault;
    }

    TComponentControlModel (TComponentModelItem item)
      : this ()
    {
      Select (item);
    }
    #endregion

    #region Members
    public void Select (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        Caption = item.TextModel.Caption;
        Width = item.LayoutModel.Width;
        Height = item.LayoutModel.Height;
        Id = item.Id;

        EntityAction.ModelAction.ExtensionGeometryModel.SizeCols = item.GeometryModel.SizeCols;
        EntityAction.ModelAction.ExtensionGeometryModel.SizeRows = item.GeometryModel.SizeRows;
      }
    }

    public void ChangeSize (TSize size)
    {
      EntityAction.ModelAction.ExtensionGeometryModel.SizeCols = size.Columns;
      EntityAction.ModelAction.ExtensionGeometryModel.SizeRows = size.Rows;
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;

        Width = alias.Width;
        Height = alias.Height;

        EntityAction.ModelAction.ExtensionGeometryModel.SizeCols = alias.EntityAction.ModelAction.ExtensionGeometryModel.SizeCols;
        EntityAction.ModelAction.ExtensionGeometryModel.SizeRows = alias.EntityAction.ModelAction.ExtensionGeometryModel.SizeRows;
      }
    }
    #endregion

    #region Static
    public static TComponentControlModel Create (TComponentModelItem item) => new TComponentControlModel (item);

    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace