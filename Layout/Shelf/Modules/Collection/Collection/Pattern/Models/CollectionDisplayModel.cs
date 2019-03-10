/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using rr.Library.Types;

using Shared.ViewModel;

using Shared.Layout.Shelf;
//---------------------------//

namespace Layout.Collection.Pattern.Models
{
  public class TCollectionDisplayModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }

    public TComponentModelItem ComponentModelItem
    {
      get;
      private set;
    }

    public Collection<TComponentModelItem> ComponentModelItemCollection
    {
      get;
      private set;
    }

    public Dictionary<TPosition, Shared.Layout.Bag.TComponentControlModel> ControlModelCollection
    {
      get;
      private set;
    }

    public bool IsEditCommandEnabled
    {
      get
      {
        return (ComponentModelItem.Id.IsEmpty ().IsFalse ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get;
      private set;
    }

    public int ChildCount
    {
      get
      {
        return (ControlModelCollection.Count);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      Cleanup ();
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        ComponentControlModel.Select (item);
        ComponentModelItem.CopyFrom (item);
      }
    }

    internal void SelectRelation (Server.Models.Component.TEntityAction action)
    {
      ComponentModelItemCollection.Clear ();

      // just one parent
      if (action.ComponentOperation.ParentIdCollection.ContainsKey (ComponentModelItem.Id)) {
        var relationCollection = action.ComponentOperation.ParentIdCollection [ComponentModelItem.Id];

        foreach (var relation in relationCollection) {
          var modelItem = TComponentModelItem.CreateDefault;
          modelItem.InfoModel.Id = relation.ChildId;
          modelItem.StatusModel.Locked = relation.Locked;
          modelItem.GeometryModel.PositionCol = relation.PositionColumn;
          modelItem.GeometryModel.PositionRow = relation.PositionRow;
          modelItem.GeometryModel.PositionIndex = relation.PositionIndex;
          modelItem.Select (Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory));

          ComponentModelItemCollection.Add (modelItem);
        }
      }
    }

    internal void SelectComponent (Server.Models.Component.TEntityAction action)
    {
      /*
       - action.CollectionAction.EntityColletion {id} -> bag (ModelAction) 
       - action.CollectionAction.EntityColletion {id}.CollectionAction.ModelCollection {id, Model} node model
      */

      ControlModelCollection.Clear ();

      foreach (var item in ComponentModelItemCollection) {
        var position = item.Position;
        var id = item.Id;
        var category = item.Category;

        // search bag
        if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
          var entityAction = action.CollectionAction.EntityCollection [id];

          // update bag model
          item.Select (entityAction.ModelAction.ComponentInfoModel);
          item.Select (entityAction.ModelAction.ExtensionLayoutModel);
          item.Select (entityAction.CollectionAction.ExtensionNodeCollection);
          item.AdjustSize ();

          // only one child (Document or Video)
          if (entityAction.CollectionAction.ModelCollection.Count.Equals (1)) {
            SelectChildNode (id, category, position, entityAction);
          }

          // many child (Image)
          else {
            SelectManyChildNode (id, category, position, entityAction);
          }
        }
      }

      IsRemoveCommandEnabled = (ComponentModelItem.CanRemove && ChildCount.Equals (0));
    }

    internal void RequestComponentId (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      foreach (var item in ComponentModelItemCollection) {
        action.IdCollection.Add (item.Id);
      }
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      action.Id = ComponentModelItem.Id; // shelf Id
      action.ModelAction.CopyFrom (ComponentModelItem.RequestModel ()); // shelf model
      action.Param1 = new Collection<TComponentModelItem> (ComponentModelItemCollection); // relation
      action.Param2 = new Dictionary<TPosition, Shared.Layout.Bag.TComponentControlModel> (ControlModelCollection); // child model
    }

    internal void SelectChildNode (Guid id, Server.Models.Infrastructure.TCategory category, TPosition position, Server.Models.Component.TEntityAction action)
    {
      // child node (just one)
      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        var childId = modelAction.Key;
        var childModel = modelAction.Value;

        var childCategory = Server.Models.Infrastructure.TCategory.None;

        var childNodes = action.CollectionAction.ExtensionNodeCollection
          .Where (p => p.ChildId.Equals (childId))
          .ToList ()
        ;

        if (childNodes.Count.Equals (1)) {
          childCategory = Server.Models.Infrastructure.TCategoryType.FromValue (childNodes [0].ChildCategory);
        }

        var childModelItem = Server.Models.Component.TComponentModel.Create (childModel);

        var childComponentModelItem = TComponentModelItem.Create (childModelItem);
        childComponentModelItem.Select (childCategory);

        //TODO: review
        //var childStyle = childComponentModelItem.Style;

        var controlModel = Shared.Layout.Bag.TComponentControlModel.CreateDefault;
        controlModel.SelectModel (id, category);
        //TODO: review
        //controlModel.SelectChildModel (childId, childCategory, childStyle, childComponentModelItem);

        ControlModelCollection.Add (position, controlModel);
      }
    }

    internal void SelectManyChildNode (Guid id, Server.Models.Infrastructure.TCategory category, TPosition position, Server.Models.Component.TEntityAction action)
    {
      var controlModel = Shared.Layout.Bag.TComponentControlModel.CreateDefault;
      controlModel.SelectModel (id, category);

      // child node 
      foreach (var modelAction in action.CollectionAction.ModelCollection) {
        var childId = modelAction.Key;
        var childModel = modelAction.Value;

        var childCategory = Server.Models.Infrastructure.TCategory.None;

        var childNodes = action.CollectionAction.ExtensionNodeCollection
          .Where (p => p.ChildId.Equals (childId))
          .ToList ()
        ;

        if (childNodes.Count.Equals (1)) {
          childCategory = Server.Models.Infrastructure.TCategoryType.FromValue (childNodes [0].ChildCategory);
        }

        var childModelItem = Server.Models.Component.TComponentModel.Create (childModel);

        var childComponentModelItem = TComponentModelItem.Create (childModelItem);
        childComponentModelItem.Select (childCategory);

        //TODO: review
        //var childStyle = childComponentModelItem.Style;
        //TODO: review
        //controlModel.SelectChildModel (childId, childCategory, childStyle, childComponentModelItem);
      }

      ControlModelCollection.Add (position, controlModel);
    }

    internal void Cleanup ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // shelf control model
      ComponentModelItem = TComponentModelItem.CreateDefault; // shelf model
      ComponentModelItemCollection = new Collection<TComponentModelItem> (); // relation model
      ControlModelCollection = new Dictionary<TPosition, Shared.Layout.Bag.TComponentControlModel> (); // child model

      IsRemoveCommandEnabled = false;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
