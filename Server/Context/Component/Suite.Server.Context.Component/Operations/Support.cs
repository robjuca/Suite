/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Linq;

using Server.Models.Infrastructure;
//---------------------------//

namespace Server.Context.Component
{
  public class TOperationSupport
  {
    #region Property
    public Guid Id
    {
      get;
    }

    public int CategoryValue
    {
      get;
    }
    #endregion

    #region Constructor
    public TOperationSupport (TModelContext context, Server.Models.Component.TEntityAction action)
      : this ()
    {
      /*
      DATA IN
      - action.Id 
      - action.CollectionAction.CategoryRelationCollection

      DATA OUT
      - action.ModelAction (model)
      - action.CollectionAction.ModeCollection {id, model} (for each node)
      */

      Id = action.Id;

      var descriptors = context.ComponentDescriptor
        .Where (p => p.Id.Equals (Id))
        .ToList ()
      ;

      // found (request Category)
      if (descriptors.Count > 0) {
        CategoryValue = descriptors [0].Category;
      }
    }

    TOperationSupport ()
    {
      Id = Guid.Empty;
      CategoryValue = TCategoryType.ToValue (TCategory.None);
    }
    #endregion

    #region Members
    public void RequestComponent (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      RequestComponent (Id, context, action, action.ModelAction);
    }

    public void RequestExtension (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      RequestExtension (CategoryValue, Id, context, action, action.ModelAction);
    }

    public void RequestNode (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
       DATA IN
      - action.Id {used as ParentId}

      DATA OUT
       - action.CollectionAction.ExtensionNodeCollection
       - action.CollectionAction.ModelCollection {id, modelAction}    // node model
      */

      var nodesCollection = context.ExtensionNode
        .Where (p => p.ParentId.Equals (action.Id))
        .ToList ()
      ;

      try {
        // node (child)
        foreach (var node in nodesCollection) {
          action.CollectionAction.ExtensionNodeCollection.Add (node);

          var id = node.ChildId;
          var categoryValue = node.ChildCategory;
          var modelAction = Server.Models.Component.TModelAction.CreateDefault;

          if (RequestComponent (id, context, action, modelAction)) {
            if (RequestExtension (categoryValue, id, context, action, modelAction)) {
              action.CollectionAction.ModelCollection.Add (id, modelAction);    // add node model
            }
          }
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("RequestNode - TOperationSupport", exception, action);
      }
    }

    public void RequestRelation (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
       DATA IN
      - action.ComponentOperation 

      DATA OUT
      - action.ComponentOperation
      */

      // Component Relation
      var componentRelationFullList = context.ComponentRelation
          .ToList ()
        ;

      // by Category
      if (action.ComponentOperation.IsComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Category)) {
        foreach (var categoryValue in action.ComponentOperation.CategoryCollection) {
          // parent 
          var parentList = componentRelationFullList
            .Where (p => p.ParentCategory.Equals (categoryValue))
            .ToList ()
          ;

          if (parentList.Count > 0) {
            action.CollectionAction.ComponentOperation.SelectParent (categoryValue, parentList);
          }

          // child
          var childList = componentRelationFullList
            .Where (p => p.ChildCategory.Equals (categoryValue))
            .ToList ()
          ;

          if (childList.Count > 0) {
            action.CollectionAction.ComponentOperation.SelectChild (categoryValue, childList);
          }
        }
      }

      // by Id
      if (action.ComponentOperation.IsComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Id)) {
        foreach (var id in action.ComponentOperation.IdCollection) {
          // parent 
          var parentList = componentRelationFullList
            .Where (p => p.ParentId.Equals (id))
            .ToList ()
          ;

          if (parentList.Count > 0) {
            action.CollectionAction.ComponentOperation.SelectParent (id, parentList);
          }

          // child
          var childList = componentRelationFullList
            .Where (p => p.ChildId.Equals (id))
            .ToList ()
          ;

          if (childList.Count > 0) {
            action.CollectionAction.ComponentOperation.SelectChild (id, childList);
          }
        }
      }
    }
    #endregion

    #region Support
    bool RequestComponent (Guid id, TModelContext context, Server.Models.Component.TEntityAction action, Server.Models.Component.TModelAction modelAction)
    {
      /*
      DATA OUT
      - action.ModelAction (model)
      */

      bool res = false;

      try {
        // info
        var infoList = context.ComponentInfo
          .Where (p => p.Id.Equals (id))
          .ToList ()
        ;

        // info found
        if (infoList.Count.Equals (1)) {
          var model = infoList [0];
          modelAction.ComponentInfoModel.CopyFrom (model);
        }

        // status
        var statusList = context.ComponentStatus
          .Where (p => p.Id.Equals (id))
          .ToList ()
        ;

        // status found
        if (statusList.Count.Equals (1)) {
          var model = statusList [0];
          modelAction.ComponentStatusModel.CopyFrom (model);
        }

        res = true;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("RequestComponent - TOperationSupport", exception, action);
      }

      return (res);
    }

    bool RequestExtension (int categoryValue, Guid id, TModelContext context, Server.Models.Component.TEntityAction action, Server.Models.Component.TModelAction modelAction)
    {
      /*
      DATA OUT
      - action.ModelAction (model)
      - action.CollectionAction.ExtensionNodeCollection
      - action.ComponentModel.NodeModelCollection
      */

      var res = false;

      // Extension (CategoryRelationCollection)
      var categoryRelationList = action.CollectionAction.CategoryRelationCollection
        .Where (p => p.Category.Equals (categoryValue))
        .ToList ()
      ;

      // found
      if (categoryRelationList.Count.Equals (1)) {
        var categoryRelation = categoryRelationList [0]; // get extension using TComponentExtension

        var extension = TComponentExtension.Create (categoryRelation.Extension);
        extension.Request ();

        try {
          foreach (var extensionName in extension.ExtensionList) {
            switch (extensionName) {
              case TComponentExtensionName.Document: {
                  var list = context.ExtensionDocument
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionDocumentModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionName.Geometry: {
                  var list = context.ExtensionGeometry
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionGeometryModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionName.Image: {
                  var list = context.ExtensionImage
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionImageModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionName.Layout: {
                  var list = context.ExtensionLayout
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionLayoutModel.CopyFrom (list [0]);
                  }
                }
                break;

              case TComponentExtensionName.Node: {
                  // child first
                  var childList = context.ExtensionNode
                    .Where (p => p.ChildId.Equals (id))
                    .ToList ()
                  ;

                  if (childList.Count.Equals (1)) {
                    var node = childList [0];
                    modelAction.ExtensionNodeModel.CopyFrom (node);

                    //  check duplicated
                    //var list = action.CollectionAction.ExtensionNodeCollection
                    //  .Where (p => p.ChildId.Equals (id))
                    //  .ToList ()
                    //;

                    //if (list.Count.Equals (0)) {
                    //  action.CollectionAction.ExtensionNodeCollection.Add (node);
                    //  action.ComponentModel.NodeModelCollection.Add (node);
                    //}
                  }

                  //else {
                  //  // parent next
                  //  //var parentList = context.ExtensionNode
                  //  //  .Where (p => p.ParentId.Equals (id))
                  //  //  .ToList ()
                  //  //;

                  //  //foreach (var node in parentList) {
                  //  //  //  check duplicated
                  //  //  var list = action.CollectionAction.ExtensionNodeCollection
                  //  //    .Where (p => p.ChildId.Equals (node.ChildId))
                  //  //    .ToList ()
                  //  //  ;

                  //  //  if (list.Count.Equals (0)) {
                  //  //    action.CollectionAction.ExtensionNodeCollection.Add (node);
                  //  //    action.ComponentModel.NodeModelCollection.Add (node);
                  //  //  }
                  //  //}
                  //}
                }
                break;

              case TComponentExtensionName.Text: {
                  var list = context.ExtensionText
                    .Where (p => p.Id.Equals (id))
                    .ToList ()
                  ;

                  if (list.Count.Equals (1)) {
                    modelAction.ExtensionTextModel.CopyFrom (list [0]);
                  }
                }
                break;
            }
          }

          res = true;
        }

        catch (Exception exception) {
          Server.Models.Infrastructure.THelper.FormatException ("RequestExtension - TOperationSupport", exception, action);
        }
      }

      return (res);
    }
    #endregion
  };
  //---------------------------//

}  // namespace