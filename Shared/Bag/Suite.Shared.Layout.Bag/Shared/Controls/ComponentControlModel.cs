﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Windows;

using rr.Library.Types;

using Shared.Types;
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

    public TStyleInfo HorizontalStyle
    {
      get;
    }

    public TStyleInfo VerticalStyle
    {
      get;
    }

    public TSize Size
    {
      get; 
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      ComponentDocumentControlModel = Shared.Gadget.Document.TComponentControlModel.CreateDefault;
      ComponentImageControlModel = Shared.Gadget.Image.TComponentControlModel.CreateDefault;

      Id = Guid.Empty;
      ChildId = Guid.Empty;

      HorizontalStyle = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      VerticalStyle = TStyleInfo.Create (TContentStyle.Mode.Vertical);

      Category = Server.Models.Infrastructure.TCategory.None;
      ChildCategory = Server.Models.Infrastructure.TCategory.None;

      Size = TSize.CreateDefault;
    }
    #endregion

    #region Members
    public void SelectModel (Guid id, Server.Models.Infrastructure.TCategory category)
    {
      if (Id.NotEquals (id)) {
        Cleanup ();

        Id = id;
        Category = category;
      }
    }

    public void SelectModel (TComponentModelItem model)
    {
      if (model.NotNull ()) {
        SelectModel (model.Id, model.Category);

        // child category
        if (model.ChildCollection.Count > 0) {
          var childModel = model.ChildCollection [0];
          var childHorizontalStyle = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
          childHorizontalStyle.Select (childModel.StyleHorizontal);

          var childVerticalStyle = TStyleInfo.Create (TContentStyle.Mode.Vertical);
          childVerticalStyle.Select (childModel.StyleVertical);

          ChildId = childModel.Id;
          ChildCategory = childModel.Category;
          HorizontalStyle.Select (childHorizontalStyle.Style);
          VerticalStyle.Select (childVerticalStyle.Style);

          switch (ChildCategory) {
            case Server.Models.Infrastructure.TCategory.Document:
              if (model.ChildCollection.Count.Equals (1)) {
                ComponentDocumentControlModel.SelectModel (childModel);
              }
              break;

            case Server.Models.Infrastructure.TCategory.Image:
              foreach (var itemChildModel in model.ChildCollection) {
                ComponentImageControlModel.SelectModel (itemChildModel);
              }
              break;

            case Server.Models.Infrastructure.TCategory.Video:
              // not now
              break;
          }

          var contentStyle = TContentStyle.CreateDefault;

          var colSize = contentStyle.RequestBoardStyleSize (HorizontalStyle.Style);
          var rowSize = contentStyle.RequestBoardStyleSize (VerticalStyle.Style);

          Size.SelectColumns (colSize);
          Size.SelectRows (rowSize);
        }
      }
    }

    public void SelectChildModel (Guid childId, Server.Models.Infrastructure.TCategory childCategory, TStyleInfo horizontalStyle, TStyleInfo verticalStyle, TComponentModelItem childModel)
    {
      ChildId = childId;
      HorizontalStyle.Select (horizontalStyle.Style);
      VerticalStyle.Select (verticalStyle.Style);
      ChildCategory = childCategory;

      switch (ChildCategory) {
        case Server.Models.Infrastructure.TCategory.Document:
          ComponentDocumentControlModel.SelectModel (childModel);
          break;

        case Server.Models.Infrastructure.TCategory.Image:
          ComponentImageControlModel.SelectModel (childModel);
          break;
      }

      var contentStyle = TContentStyle.CreateDefault;

      var colSize = contentStyle.RequestBoardStyleSize (HorizontalStyle.Style);
      var rowSize = contentStyle.RequestBoardStyleSize (VerticalStyle.Style);

      Size.SelectColumns (colSize);
      Size.SelectRows (rowSize);
    }

    public void RequestChildComponentModel (List<TComponentModelItem> models)
    {
      if (models.NotNull ()) {
        switch (ChildCategory) {
          case Server.Models.Infrastructure.TCategory.Document:
            ComponentDocumentControlModel.RequestComponentModel (models);
            break;

          case Server.Models.Infrastructure.TCategory.Image:
            ComponentImageControlModel.RequestComponentModel (models);
            break;

          case Server.Models.Infrastructure.TCategory.Video:
            break;
        }
      }
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        Id = alias.Id;
        ChildId = alias.ChildId;

        HorizontalStyle.Select (alias.HorizontalStyle.Style);
        VerticalStyle.Select (alias.VerticalStyle.Style);

        Category = alias.Category;
        ChildCategory = alias.ChildCategory;

        Size.CopyFrom (alias.Size);

        ComponentDocumentControlModel.CopyFrom (alias.ComponentDocumentControlModel);
        ComponentImageControlModel.CopyFrom (alias.ComponentImageControlModel);
      }
    }

    public void Cleanup ()
    {
      Id = Guid.Empty;
      ChildId = Guid.Empty;

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