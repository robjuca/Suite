/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;

using rr.Library.Types;

using Server.Models.Component;
//---------------------------//

namespace Shared.ViewModel
{
  public sealed class TComponentModelItem : TComponentModel
  {
    #region Property
    #region Visibility
    public Visibility DisableVisibility
    {
      get
      {
        return (Enabled ? Visibility.Collapsed : Visibility.Visible);
      }
    }

    public Visibility EnableVisibility
    {
      get
      {
        return (Enabled ? Visibility.Visible : Visibility.Collapsed);
      }
    }

    public Visibility DistortedVisibility
    {
      get
      {
        return (IsCategoryImage ? (ImageModel.Distorted ? Visibility.Visible : Visibility.Collapsed) : Visibility.Collapsed);
      }
    }

    public Visibility PictureVisibility
    {
      get
      {
        return (IsCategoryImage ? (ImageModel.Image.IsNull () ? Visibility.Collapsed : ImageModel.Distorted ? Visibility.Collapsed : Visibility.Visible) : Visibility.Collapsed);
      }
    }

    public Visibility ActiveVisibility
    {
      get
      {
        return (Active ? (Enabled ? Visibility.Visible : Visibility.Collapsed) : Visibility.Collapsed);
      }
    }

    public Visibility BusyVisibility
    {
      get
      {
        return (Busy ? Visibility.Visible : Visibility.Collapsed);
      }
    }
    #endregion

    #region Sring
    public string StyleString
    {
      get
      {
        return ($"{LayoutModel.StyleHorizontal},{LayoutModel.StyleVertical} : {LayoutModel.Width} x {LayoutModel.Height}");
      }
    }

    public string ImageString
    {
      get
      {
        return (IsCategoryImage ? $"{GeometryModel.PositionImage} {ImageModel.Width} x {ImageModel.Height}" : string.Empty);
      }
    }

    public string SizeString
    {
      get
      {
        return ($"cols: {GeometryModel.SizeCols}  rows: {GeometryModel.SizeRows}");
      }
    }

    public string LinkString
    {
      get
      {
        return ($"{TextModel.ExternalLink}");
      }
    } 
    #endregion

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
      private set;
    }

    public bool IsCategoryImage
    {
      get
      {
        return (Category.Equals (Server.Models.Infrastructure.TCategory.Image));
      }
    }

    public bool Locked
    {
      get
      {
        return (StatusModel.Locked);
      }
    }

    public bool Busy
    {
      get
      {
        return (StatusModel.Busy);
      }
    }

    public bool Active
    {
      get
      {
        return (StatusModel.Active);
      }
    }

    public bool Enabled
    {
      get
      {
        return (InfoModel.Enabled);
      }
    }

    public bool CanRemove
    {
      get
      {
        return (Enabled.IsFalse () && HasChild.IsFalse () && ValidateId);
      }
    }

    public bool ValidateId
    {
      get
      {
        return (Id.IsEmpty ().IsFalse ());
      }
    }

    public TSize Size
    {
      get
      {
        return (TSize.Create (GeometryModel.SizeCols, GeometryModel.SizeRows));
      }
    }

    public TPosition Position
    {
      get
      {
        return (TPosition.Create (GeometryModel.PositionCol, GeometryModel.PositionRow));
      }
    }

    public Collection<TComponentModelItem> ChildCollection
    {
      get; 
    }

    public bool HasChild
    {
      get
      {
        return (ChildCollection.Count > 0);
      }
    }

    public bool HasRelation
    {
      get
      {
        return (
          Category.Equals (Server.Models.Infrastructure.TCategory.Shelf) || 
          Category.Equals (Server.Models.Infrastructure.TCategory.Drawer) || 
          Category.Equals (Server.Models.Infrastructure.TCategory.Chest)
        );
      }
    }
    #endregion

    #region Constructor
    TComponentModelItem ()
    {
      Category = Server.Models.Infrastructure.TCategory.None;

      ChildCollection = new Collection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    public void Select (Server.Models.Infrastructure.TCategory category)
    {
      if (category.NotNull ()) {
        Category = category;
      }
    }

    public void Select (Guid id, Server.Models.Infrastructure.TCategory category)
    {
      InfoModel.Id = id;

      Select (category);
    }

    public void CopyFrom (TComponentModelItem alias)
    {
      if (alias.NotNull ()) {
        base.CopyFrom (alias);

        Category = alias.Category;

        ChildCollection.Clear ();

        foreach (var item in alias.ChildCollection) {
          ChildCollection.Add (item);
        }
      }
    }

    public void CopyFromBase (TComponentModel alias)
    {
      if (alias.NotNull ()) {
        CopyFrom (alias);
      }
    }

    public void AdjustSize ()
    {
      //TODO: review
      //var style = TContentStyle.TryToParse (Style);

      //switch (style) {
      //  case TContentStyle.Style.mini: {
      //      GeometryModel.SizeCols = 1;
      //      GeometryModel.SizeRows = 1;
      //    }
      //    break;

      //  case TContentStyle.Style.small: {
      //      GeometryModel.SizeCols = 1;
      //      GeometryModel.SizeRows = 2;
      //    }
      //    break;

      //  case TContentStyle.Style.large: {
      //      GeometryModel.SizeCols = 1;
      //      GeometryModel.SizeRows = 3;
      //    }
      //    break;

      //  case TContentStyle.Style.big: {
      //      GeometryModel.SizeCols = 2;
      //      GeometryModel.SizeRows = 3;
      //    }
      //    break;
      //}
    }

    public void RequestChild (TEntityAction action)
    {
      if (action.NotNull ()) {
        if (HasRelation) {
          RequestRelation (action);
        }

        // use Nodes
        else {
          RequestNode (action);
        }

        foreach (var child in ChildCollection) {
          if (action.CollectionAction.EntityCollection.ContainsKey (child.Id)) {
            var childAction = action.CollectionAction.EntityCollection [child.Id]; // child action
            child.RequestChild (childAction);
          }
        }
      }
    }

    public void RequestRelation (TEntityAction action)
    {
      if (action.ComponentOperation.ParentIdCollection.ContainsKey (Id)) {
        var relationCollection = action.ComponentOperation.ParentIdCollection [Id]; // relation

        //  child relation 
        foreach (var relation in relationCollection) {
          if (action.CollectionAction.EntityCollection.ContainsKey (relation.ChildId)) {
            var childAction = action.CollectionAction.EntityCollection [relation.ChildId]; // child action

            // child model
            var childModelItem = Create (childAction);
            childModelItem.GeometryModel.PositionCol = relation.PositionColumn;
            childModelItem.GeometryModel.PositionRow = relation.PositionRow;
            childModelItem.GeometryModel.PositionIndex = relation.PositionIndex;
            childModelItem.Select (Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory));

            ChildCollection.Add (childModelItem); // add to child collection 
          }
        }
      }
    }

    public void RequestNode (TEntityAction action)
    {
      foreach (var node in action.CollectionAction.ExtensionNodeCollection) {
        NodeModelCollection.Add (node);

        if (action.CollectionAction.ModelCollection.ContainsKey (node.ChildId)) {
          var childNodeModel = action.CollectionAction.ModelCollection [node.ChildId]; // node model
          var childNodeComponentModel = Create (childNodeModel); // child node 

          var childNodeComponentModelItem = Create (childNodeComponentModel);
          childNodeComponentModelItem.Select (Server.Models.Infrastructure.TCategoryType.FromValue (node.ChildCategory));

          ChildCollection.Add (childNodeComponentModelItem);
        }
      }
    }

    public void ClearActiveStatus ()
    {
      StatusModel.Active = false;
    }

    public void SelectActiveStatus (bool active)
    {
      StatusModel.Active = active;
    }
    #endregion

    #region Static
    public static TComponentModelItem Create (TComponentModel model)
    {
      var modelItem = CreateDefault;

      if (model.NotNull ()) {
        modelItem.CopyFrom (model);
        modelItem.AdjustSize ();  // Size adjust (Document, Image, Video, Bag) these use Style
      }

      return (modelItem);
    }

    public static TComponentModelItem Create (TEntityAction action)
    {
      var modelItem = CreateDefault;

      if (action.NotNull ()) {
        var model = Create (action.ModelAction);

        modelItem.CopyFrom (model);
        modelItem.AdjustSize ();  // Size adjust (Document, Image, Video, Bag) these use Style
        modelItem.Select (action.CategoryType.Category);
      }

      return (modelItem);
    }

    public static TComponentModelItem CreateDefault => new TComponentModelItem ();
    #endregion
  };
  //---------------------------//

}  // namespace