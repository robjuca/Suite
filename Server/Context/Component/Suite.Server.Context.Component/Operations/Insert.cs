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
  public sealed class TOperationInsert : IOperation
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
        Server.Models.Infrastructure.THelper.FormatExtensionNotImplementedException (action);
      }

      else {
        Insert (context, action);
      }
    }
    #endregion

    #region Support
    void Insert (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
       DATA IN:
       - action.Operation.CategoryType.Category
       - action.CollectionAction.CategoryRelationCollection
       - action.ModelAction 
       */

      try {
        // Validate Name
        if (ValidateString (action)) {
          //Id
          var id = Guid.NewGuid ();
          var categoryValue = TCategoryType.ToValue (action.Operation.CategoryType.Category);

          // Descriptor
          action.ModelAction.ComponentDescriptorModel.Id = id;
          action.ModelAction.ComponentDescriptorModel.Category = categoryValue;

          var compDescriptor = Server.Models.Component.ComponentDescriptor.CreateDefault;
          compDescriptor.CopyFrom (action.ModelAction.ComponentDescriptorModel);

          context.ComponentDescriptor.Add (compDescriptor);

          // Info
          action.ModelAction.ComponentInfoModel.Id = id;

          var compInfo = Server.Models.Component.ComponentInfo.CreateDefault;
          compInfo.CopyFrom (action.ModelAction.ComponentInfoModel);

          context.ComponentInfo.Add (compInfo);

          // Status
          action.ModelAction.ComponentStatusModel.Id = id;

          var compStatus = Server.Models.Component.ComponentStatus.CreateDefault;
          compStatus.CopyFrom (action.ModelAction.ComponentStatusModel);

          context.ComponentStatus.Add (compStatus);

          // status collection
          foreach (var item in action.CollectionAction.ComponentStatusCollection) {
            var list = context.ComponentStatus
              .Where (p => p.Id.Equals (item.Id))
              .ToList ()
            ;

            // already exist (update)
            if (list.Count.Equals (1)) {
              var model = list [0];
              model.Change (item);

              context.ComponentStatus.Update (model);
            }

            // new (add)
            else {
              context.ComponentStatus.Add (item);
            }
          }

          // Component Relation Collection
          foreach (var item in action.CollectionAction.ComponentRelationCollection) {
            // change child status busy to true
            var childList = context.ComponentStatus
              .Where (p => p.Id.Equals (item.ChildId))
              .ToList ()
            ;

            // found
            if (childList.Count.Equals (1)) {
              var child = childList [0];
              child.Busy = true;

              context.ComponentStatus.Update (child); // update
            }

            item.ParentId = id;
            context.ComponentRelation.Add (item); // insert new
          }

          // extensions

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

            foreach (var extensionName in extension.ExtensionList) {
              switch (extensionName) {
                case TComponentExtensionName.Document: {
                    action.ModelAction.ExtensionDocumentModel.Id = id;

                    var extDocument = Server.Models.Component.ExtensionDocument.CreateDefault;
                    extDocument.CopyFrom (action.ModelAction.ExtensionDocumentModel);

                    context.ExtensionDocument.Add (extDocument);
                  }
                  break;

                case TComponentExtensionName.Geometry: {
                    action.ModelAction.ExtensionGeometryModel.Id = id;

                    var extGeometry = Server.Models.Component.ExtensionGeometry.CreateDefault;
                    extGeometry.CopyFrom (action.ModelAction.ExtensionGeometryModel);

                    context.ExtensionGeometry.Add (extGeometry);
                  }
                  break;

                case TComponentExtensionName.Image: {
                    action.ModelAction.ExtensionImageModel.Id = id;

                    var extImage = Server.Models.Component.ExtensionImage.CreateDefault;
                    extImage.CopyFrom (action.ModelAction.ExtensionImageModel);

                    context.ExtensionImage.Add (extImage);
                  }
                  break;

                case TComponentExtensionName.Layout: {
                    action.ModelAction.ExtensionLayoutModel.Id = id;

                    var extLayout = Server.Models.Component.ExtensionLayout.CreateDefault;
                    extLayout.CopyFrom (action.ModelAction.ExtensionLayoutModel);

                    context.ExtensionLayout.Add (extLayout);
                  }
                  break;

                case TComponentExtensionName.Node: {
                    // Use Node Collection
                    foreach (var nodeModel in action.CollectionAction.ExtensionNodeCollection) {
                      nodeModel.ParentId = id;

                      context.ExtensionNode.Add (nodeModel);
                    }
                  }
                  break;

                case TComponentExtensionName.Text: {
                    action.ModelAction.ExtensionTextModel.Id = id;

                    var extText = Server.Models.Component.ExtensionText.CreateDefault;
                    extText.CopyFrom (action.ModelAction.ExtensionTextModel);

                    context.ExtensionText.Add (extText);
                  }
                  break;
              }
            }
          }

          context.SaveChanges ();

          action.Result = TValidationResult.Success;
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Insert", exception, action);
      }
    }

    bool ValidateString (Server.Models.Component.TEntityAction action)
    {
      if (string.IsNullOrEmpty (action.ModelAction.ComponentInfoModel.Name.Trim ())) {
        action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Insert] Name can NOT be NULL or EMPTY!");
        return (false);
      }

      return (true);
    }
    #endregion
  };
  //---------------------------//

}  // namespace