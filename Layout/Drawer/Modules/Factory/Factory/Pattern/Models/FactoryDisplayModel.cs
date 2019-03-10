/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.ViewModel;

using Shared.Layout.Drawer;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryDisplayModel
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
    #endregion

    #region Constructor
    public TFactoryDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // control model
      ComponentModelItem = TComponentModelItem.CreateDefault; // model
    }
    #endregion

    #region Members
    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      //action.ModelAction {model}
      //action.Param1 = Collection<TComponentModelItem> {child collection}

      action.ThrowNull ();

      var model = Server.Models.Component.TComponentModel.Create (action.ModelAction);
      var modelItem = TComponentModelItem.Create (model);
      modelItem.Select (action.CategoryType.Category);

      ComponentControlModel.Select (modelItem);
      ComponentModelItem.CopyFrom (modelItem);

      if (action.Param1 is Collection<TComponentModelItem> childList) {
        ComponentModelItem.ChildCollection.Clear ();

        foreach (var item in childList) {
          ComponentModelItem.ChildCollection.Add (item);
        }
      }
    }

    //internal void SelectRelation (Server.Models.Component.TEntityAction action)
    //{
    //  ComponentModelItemCollection.Clear ();

    //  // just one parent
    //  if (action.ComponentOperation.ParentIdCollection.ContainsKey (ComponentModelItem.Id)) {
    //    var relationCollection = action.ComponentOperation.ParentIdCollection [ComponentModelItem.Id];

    //    foreach (var relation in relationCollection) {
    //      var modelItem = TComponentModelItem.CreateDefault;
    //      modelItem.InfoModel.Id = relation.ChildId;
    //      modelItem.StatusModel.Locked = relation.Locked;
    //      modelItem.GeometryModel.PositionCol = relation.PositionColumn;
    //      modelItem.GeometryModel.PositionRow = relation.PositionRow;
    //      modelItem.GeometryModel.PositionIndex = relation.PositionIndex;
    //      modelItem.Select (Server.Models.Infrastructure.TCategoryType.FromValue (relation.ChildCategory));

    //      ComponentModelItemCollection.Add (modelItem);
    //    }
    //  }
    //}

    //internal void SelectComponent (Server.Models.Component.TEntityAction action)
    //{
    //  /*
    //   - action.CollectionAction.EntityColletion {id} ->  (ModelAction) 
    //   - action.CollectionAction.EntityColletion {id}.ComponentModel {NodeModelCollection}
    //   - action.CollectionAction.EntityColletion {id}.ComponentOperation {node}
    //  */

    //  foreach (var item in ComponentModelItemCollection) {
    //    var id = item.Id;

    //    // search 
    //    if (action.CollectionAction.EntityCollection.ContainsKey (id)) {
    //      var entityAction = action.CollectionAction.EntityCollection [id];

    //      // update model {shelf}
    //      item.Select (entityAction.ModelAction.ComponentInfoModel);
    //      item.Select (entityAction.ModelAction.ExtensionLayoutModel);
    //      item.GeometryModel.SizeCols = entityAction.ModelAction.ExtensionGeometryModel.SizeCols;
    //      item.GeometryModel.SizeRows = entityAction.ModelAction.ExtensionGeometryModel.SizeRows;

    //      RequestRelations (item, entityAction);
    //    }
    //  }
    //}

    //internal void RequestComponentId (Server.Models.Component.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  foreach (var item in ComponentModelItemCollection) {
    //    action.IdCollection.Add (item.Id);
    //  }
    //}

    //internal void RequestModel (Server.Models.Component.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  action.Id = ComponentModelItem.Id; // shelf Id
    //  action.ModelAction.CopyFrom (ComponentModelItem.RequestModel ()); // shelf model
    //  action.Param1 = new Collection<TComponentModelItem> (ComponentModelItemCollection.ToList ()); // child collection
    //}

    //internal Collection<TComponentModelItem> RequestChildCollection ()
    //{
    //  return (new Collection<TComponentModelItem> (ComponentModelItemCollection.ToList ())); // child collection
    //}

    internal void Cleanup ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // control model
      ComponentModelItem = TComponentModelItem.CreateDefault; // model
    }
    #endregion

    #region Support
    //void RequestRelations (TComponentModelItem item, Server.Models.Component.TEntityAction action)
    //{
    //  var id = item.Id;
    //  var position = item.Position;
    //  var category = item.Category;

    //  // child node {- action.CollectionAction.EntityColletion {id}.ComponentOperation {ParentIdCollection}} {bag}
    //  if (action.ComponentOperation.ParentIdCollection.ContainsKey (id)) {
    //    var componentRelationList = action.ComponentOperation.ParentIdCollection [id];

    //    item.ChildCollection.Clear ();

    //    foreach (var componentRelation in componentRelationList) {
    //      var childId = componentRelation.ChildId;
    //      var childCategory = Server.Models.Infrastructure.TCategoryType.FromValue (componentRelation.ChildCategory);

    //      // child model (shelf contains bag)
    //      if (action.CollectionAction.EntityCollection.ContainsKey (childId)) {
    //        var actionChild = action.CollectionAction.EntityCollection [childId];

    //        var childComponentModelBase = Server.Models.Component.TComponentModel.Create (actionChild.ModelAction);
    //        var childComponentModelItem = TComponentModelItem.Create (childComponentModelBase);
    //        childComponentModelItem.Select (childCategory);
    //        childComponentModelItem.GeometryModel.PositionCol = componentRelation.PositionColumn;
    //        childComponentModelItem.GeometryModel.PositionRow = componentRelation.PositionRow;

    //        RequestChildren (childComponentModelItem, actionChild);

    //        item.ChildCollection.Add (childComponentModelItem);
    //      }
    //    }
    //  }
    //}

    //void RequestChildren (TComponentModelItem item, Server.Models.Component.TEntityAction action)
    //{
    //  item.ChildCollection.Clear ();

    //  // bag child nodes (Document or Image or Video) as children
    //  foreach (var modelAction in action.CollectionAction.ModelCollection) {
    //    var childrenId = modelAction.Key;
    //    var childrenModel = modelAction.Value;

    //    var childrenCategory = Server.Models.Infrastructure.TCategory.None;

    //    var childrenNodes = action.CollectionAction.ExtensionNodeCollection
    //      .Where (p => p.ChildId.Equals (childrenId))
    //      .ToList ()
    //    ;

    //    foreach (var node in childrenNodes) {
    //      item.NodeModelCollection.Add (node);
    //    }

    //    // just one node as child
    //    if (childrenNodes.Count.Equals (1)) {
    //      var children = childrenNodes [0];
    //      childrenCategory = Server.Models.Infrastructure.TCategoryType.FromValue (children.ChildCategory);
    //    }

    //    var childrenModelItem = Server.Models.Component.TComponentModel.Create (childrenModel);
    //    var childrenComponentModelItem = TComponentModelItem.Create (childrenModelItem);
    //    childrenComponentModelItem.Select (childrenCategory);

    //    item.ChildCollection.Add (childrenComponentModelItem);
    //  }
    //}
    #endregion
  };
  //---------------------------//

}  // namespace