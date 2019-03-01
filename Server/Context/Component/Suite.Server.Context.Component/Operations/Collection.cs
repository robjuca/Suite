/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Linq;

using rr.Library.Helper;

using Server.Models.Infrastructure;
//---------------------------//

namespace Server.Context.Component
{
  public sealed class TOperationCollection : IOperation
  {
    #region Interface
    public void Invoke (IModelContext modelContext, IEntityAction entityAction, Server.Models.Infrastructure.TExtension extension)
    {
      var context = TModelContext.CastTo (modelContext);

      var relationList = context.CategoryRelation
        .ToList ()
      ;

      var action = Server.Models.Component.TEntityAction.Request (entityAction);
      action.CollectionAction.SetCollection (relationList);

      if (action.Operation.HasExtension) {
        switch (extension) {
          case Models.Infrastructure.TExtension.Full: {
              CollectionFull (context, action);
            }
            break;

          case Models.Infrastructure.TExtension.Minimum: {
              CollectionMinimum (context, action);
            }
            break;

          case Models.Infrastructure.TExtension.ById:
          case Models.Infrastructure.TExtension.Idle:
          case Models.Infrastructure.TExtension.Many:
          case Models.Infrastructure.TExtension.Zap: {
              Server.Models.Infrastructure.THelper.FormatExtensionNotImplementedException (action);
            }
            break;
        }
      }

      else {
        Server.Models.Infrastructure.THelper.FormatExtensionMustExistException (action);
      }
    }
    #endregion

    #region Support
    void CollectionFull (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /* 
        DATA IN:
          action.Operation.CategoryType.Category  
          action.CollectionAction.CategoryRelationCollection
      */

      try {
        // select Id by Category
        var categoryValue = TCategoryType.ToValue (action.Operation.CategoryType.Category);

        var descriptors = context.ComponentDescriptor
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        // found
        if (descriptors.Count > 0) {
          // Component Info, Status
          action.CollectionAction.ComponentInfoCollection.Clear ();
          action.CollectionAction.ComponentStatusCollection.Clear ();

          foreach (var descriptor in descriptors) {
            // Info
            var infoList = context.ComponentInfo
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // Status
            var statusList = context.ComponentStatus
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // info found
            if (infoList.Count.Equals (1)) {
              var infoModel = infoList [0];
              action.CollectionAction.ComponentInfoCollection.Add (infoModel);
            }

            // status found
            if (statusList.Count.Equals (1)) {
              var statusModel = statusList [0];
              action.CollectionAction.ComponentStatusCollection.Add (statusModel);
            }
          }

          // Component Relation
          // by Category
          action.CollectionAction.ComponentOperation.Clear ();
          action.CollectionAction.SelectComponentOperation (Models.Component.TComponentOperation.TInternalOperation.Category);
          action.CollectionAction.ComponentOperation.SelectByCategory (categoryValue);

          var componentRelationFullList = context.ComponentRelation
            .ToList ()
          ;

          foreach (var itemCategory in action.CollectionAction.ComponentOperation.CategoryCollection) {
            // parent 
            var parentList = componentRelationFullList
              .Where (p => p.ParentCategory.Equals (itemCategory))
              .ToList ()
            ;

            action.CollectionAction.ComponentOperation.SelectParent (itemCategory, parentList);

            // child
            var childList = componentRelationFullList
              .Where (p => p.ChildCategory.Equals (itemCategory))
              .ToList ()
            ;

            action.CollectionAction.ComponentOperation.SelectChild (itemCategory, childList);
          }

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

            foreach (var item in action.CollectionAction.ComponentInfoCollection) {
              var id = item.Id;

              foreach (var extensionName in extension.ExtensionList) {
                switch (extensionName) {
                  case TComponentExtensionName.Document: {
                      var list = context.ExtensionDocument
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionDocumentCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionName.Geometry: {
                      var list = context.ExtensionGeometry
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionGeometryCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionName.Image: {
                      var list = context.ExtensionImage.Where (p => p.Id.Equals (id)).ToList ();

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionImageCollection.Add (list [0]);
                      }
                    }
                    break;

                  case TComponentExtensionName.Layout: {
                      var list = context.ExtensionLayout
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionLayoutCollection.Add (list [0]);
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
                        action.ModelAction.ExtensionNodeModel.CopyFrom (childList [0]);
                        action.CollectionAction.ExtensionNodeCollection.Add (childList [0]);
                      }

                      else {
                        // parent next
                        var parentList = context.ExtensionNode
                          .Where (p => p.ParentId.Equals (id))
                          .ToList ()
                        ;

                        foreach (var model in parentList) {
                          action.CollectionAction.ExtensionNodeCollection.Add (model);
                        }
                      }
                    }
                    break;

                  case TComponentExtensionName.Text: {
                      var list = context.ExtensionText
                        .Where (p => p.Id.Equals (id))
                        .ToList ()
                      ;

                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionTextCollection.Add (list [0]);
                      }
                    }
                    break;
                }
              }
            }
          }

          // populate ModelCollection
          action.CollectionAction.ModelCollection.Clear ();

          var componentExtension = TComponentExtension.CreateDefault;
          action.RequestExtension (componentExtension);


          foreach (var item in action.CollectionAction.ComponentInfoCollection) {
            // component
            // Info
            var id = item.Id;
            var models = Server.Models.Component.TModelAction.CreateDefault;

            models.ComponentInfoModel.CopyFrom (item);

            // Status
            var statusList = action.CollectionAction.ComponentStatusCollection
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // found
            if (statusList.Count.Equals (1)) {
              models.ComponentStatusModel.CopyFrom (statusList [0]);
            }

            // extension
            foreach (var extensionName in componentExtension.ExtensionList) {
              switch (extensionName) {
                case TComponentExtensionName.Document: {
                    var list = action.CollectionAction.ExtensionDocumentCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionDocumentModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionName.Geometry: {
                    var list = action.CollectionAction.ExtensionGeometryCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionGeometryModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionName.Image: {
                    var list = action.CollectionAction.ExtensionImageCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionImageModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionName.Layout: {
                    var list = action.CollectionAction.ExtensionLayoutCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionLayoutModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionName.Node: {
                    var list = action.CollectionAction.ExtensionNodeCollection
                      .Where (p => p.ChildId.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionNodeModel.CopyFrom (list [0]);
                    }
                  }
                  break;

                case TComponentExtensionName.Text: {
                    var list = action.CollectionAction.ExtensionTextCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionTextModel.CopyFrom (list [0]);
                    }
                  }
                  break;
              }
            }

            action.CollectionAction.ModelCollection.Add (id, models);
          }
        }

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Collection Full", exception, action);
      }
    }

    void CollectionMinimum (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /* 
        DATA IN:
          action.Operation.CategoryType.Category  
          action.CollectionAction.CategoryRelationCollection
      */

      try {
        // select Id by Category
        var categoryValue = TCategoryType.ToValue (action.Operation.CategoryType.Category);

        var descriptors = context.ComponentDescriptor
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        // found
        if (descriptors.Count > 0) {
          // Component Info, Status
          action.CollectionAction.ComponentInfoCollection.Clear ();
          action.CollectionAction.ComponentStatusCollection.Clear ();

          foreach (var descriptor in descriptors) {
            // Info
            var infoList = context.ComponentInfo
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // Status
            var statusList = context.ComponentStatus
              .Where (p => p.Id.Equals (descriptor.Id))
              .ToList ()
            ;

            // info found
            if (infoList.Count.Equals (1)) {
              var infoModel = infoList [0];
              action.CollectionAction.ComponentInfoCollection.Add (infoModel);
            }

            // status found
            if (statusList.Count.Equals (1)) {
              var statusModel = statusList [0];
              action.CollectionAction.ComponentStatusCollection.Add (statusModel);
            }
          }

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

            foreach (var item in action.CollectionAction.ComponentInfoCollection) {
              var id = item.Id;

              foreach (var extensionName in extension.ExtensionList) {
                switch (extensionName) {
                  case TComponentExtensionName.Layout: {
                      var list = context.ExtensionLayout
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;


                      if (list.Count.Equals (1)) {
                        action.CollectionAction.ExtensionLayoutCollection.Add (list [0]);
                      }
                    }
                    break;
                }
              }
            }
          }

          // populate ModelCollection
          action.CollectionAction.ModelCollection.Clear ();

          var componentExtension = TComponentExtension.CreateDefault;
          action.RequestExtension (componentExtension);


          foreach (var item in action.CollectionAction.ComponentInfoCollection) {
            // component
            // Info
            var id = item.Id;
            var models = Server.Models.Component.TModelAction.CreateDefault;

            models.ComponentInfoModel.CopyFrom (item);

            // Status
            var statusList = action.CollectionAction.ComponentStatusCollection
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // found
            if (statusList.Count.Equals (1)) {
              models.ComponentStatusModel.CopyFrom (statusList [0]);
            }

            // extension
            foreach (var extensionName in componentExtension.ExtensionList) {
              switch (extensionName) {
                case TComponentExtensionName.Layout: {
                    var list = action.CollectionAction.ExtensionLayoutCollection
                      .Where (p => p.Id.Equals (id))
                      .ToList ()
                    ;

                    if (list.Count.Equals (1)) {
                      models.ExtensionLayoutModel.CopyFrom (list [0]);
                    }
                  }
                  break;
              }
            }

            action.CollectionAction.ModelCollection.Add (id, models);
          }
        }

        action.Result = TValidationResult.Success;
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Collection Full", exception, action);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace