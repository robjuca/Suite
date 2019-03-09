/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Layout.Bag
{
  public class TComponentControlModel
  {
    #region Property
    public Shared.Gadget.Document.TComponentControlModel ComponentDocumentControlModel
    {
      get;
      private set;
    }

    public Shared.Gadget.Image.TComponentControlModel ComponentImageControlModel
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.TCategory ChildCategory
    {
      get;
      private set;
    }

    public Visibility DocumentVisibility
    {
      get
      {
        return (ChildCategory.Equals (Server.Models.Infrastructure.TCategory.Document) ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public Visibility ImageVisibility
    {
      get
      {
        return (ChildCategory.Equals (Server.Models.Infrastructure.TCategory.Image) ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public Guid Id
    {
      get;
      private set;
    }

    public Guid ChildId
    {
      get;
      private set;
    }

    public string Style
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ComponentDocumentControlModel = Shared.Gadget.Document.TComponentControlModel.CreateDefault;
      ComponentImageControlModel = Shared.Gadget.Image.TComponentControlModel.CreateDefault;

      Id = Guid.Empty;
      ChildId = Guid.Empty;

      Style = string.Empty;

      Category = Server.Models.Infrastructure.TCategory.None;
      ChildCategory = Server.Models.Infrastructure.TCategory.None;
    }
    #endregion

    #region Members
    public void SelectModel (Guid id, Server.Models.Infrastructure.TCategory category)
    {
      Cleanup ();

      Id = id;
      Category = category;
    }

    public void SelectChildModel (Guid childId, Server.Models.Infrastructure.TCategory childCategory, string childStyle, TComponentModelItem childModel)
    {
      ChildId = childId;
      Style = childStyle;
      ChildCategory = childCategory;

      switch (ChildCategory) {
        case Server.Models.Infrastructure.TCategory.Document:
          ComponentDocumentControlModel.SelectModel (childModel);
          break;

        case Server.Models.Infrastructure.TCategory.Image:
          ComponentImageControlModel.SelectModel (childModel);
          break;
      }
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        ChildId = alias.ChildId;

        Style = alias.Style;

        Category = alias.Category;
        ChildCategory = alias.ChildCategory;

        ComponentDocumentControlModel.CopyFrom (alias.ComponentDocumentControlModel);
        ComponentImageControlModel.CopyFrom (alias.ComponentImageControlModel);
      }
    }

    public void Cleanup ()
    {
      Id = Guid.Empty;
      ChildId = Guid.Empty;

      Style = string.Empty;

      Category = Server.Models.Infrastructure.TCategory.None;
      ChildCategory = Server.Models.Infrastructure.TCategory.None;

      ComponentDocumentControlModel.Cleanup ();
      ComponentImageControlModel.Cleanup ();
    } 
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace