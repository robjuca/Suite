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
  public sealed class TOperationChange : Server.Models.Infrastructure.IOperation
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
          case Models.Infrastructure.TExtension.Settings: {
              ChangeSettings (context, action);
            }
            break;

          case Models.Infrastructure.TExtension.Full: {
              ChangeFull (context, action);
            }
            break;

          case Models.Infrastructure.TExtension.Active: {
              ChangeActive (context, action);
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
    void ChangeSettings (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
      DATA IN
      - action.ModelAction (Settings model)
      */

      try {
        var modelList = context.Settings
        .ToList ()
      ;

        // only one record
        if (modelList.Count.Equals (1)) {
          var model = modelList [0];
          model.Change (action.ModelAction.SettingsModel);

          context.Settings.Update (model);// change Settings model
          context.SaveChanges (); // done

          action.Result = TValidationResult.Success;
        }

        // wrong record count
        else {
          action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Change Settings] Wrong record count!");
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Change - Settings", exception, action);
      }
    }

    void ChangeFull (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
      DATA IN
      - action.Id (Component id to change)
      - action.ModelAction (Component model)
      */

      //validate Name
      if (ValidateString (action)) {
        var id = action.Id;

        try {
          //Component Id must exist
          if (id.IsEmpty ()) {
            action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Change Full] Component Id can NOT be NULL or EMPTY!");
          }

          else {
            // search Id
            var descriptors = context.ComponentDescriptor
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // Descriptor found
            if (descriptors.Count.Equals (1)) {
              var descriptor = descriptors [0];
              var categoryValue = descriptor.Category;

              // Info
              var infoList = context.ComponentInfo
                .Where (p => p.Id.Equals (id))
                .ToList ()
              ;

              // Info found
              if (infoList.Count.Equals (1)) {
                var model = infoList [0];
                model.Change (action.ModelAction.ComponentInfoModel);
                context.ComponentInfo.Update (model);// change Info model
              }

              // Status 
              var statusList = context.ComponentStatus
                .Where (p => p.Id.Equals (id))
                .ToList ()
              ;

              // Status found
              if (statusList.Count.Equals (1)) {
                var model = statusList [0];
                model.Change (action.ModelAction.ComponentStatusModel);
                context.ComponentStatus.Update (model);// change Status model
              }

              // status collection
              foreach (var item in action.CollectionAction.ComponentStatusCollection) {
                var list = context.ComponentStatus
                  .Where (p => p.Id.Equals (item.Id))
                  .ToList ()
                ;

                // Status found
                if (list.Count.Equals (1)) {
                  var model = list [0];
                  model.Change (item);
                  context.ComponentStatus.Update (model);// change Status model
                }
              }

              context.SaveChanges (); // save here

              // Component Relation Collection

              //remove old first (Parent)
              var relationList = context.ComponentRelation
                .Where (p => p.ParentId.Equals (id))
                .ToList ()
              ;

              foreach (var relation in relationList) {
                // change old child status busy to false
                var childList = context.ComponentStatus
                  .Where (p => p.Id.Equals (relation.ChildId))
                  .ToList ()
                ;

                // found
                if (childList.Count.Equals (1)) {
                  var child = childList [0];
                  child.Busy = false;

                  context.ComponentStatus.Update (child); // update
                }

                context.ComponentRelation.Remove (relation); // remove old
              }

              // insert new
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

              // search for extensions
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
                        var list = context.ExtensionDocument
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          var model = list [0];
                          model.Change (action.ModelAction.ExtensionDocumentModel);
                          context.ExtensionDocument.Update (model); // change model
                        }
                      }
                      break;

                    case TComponentExtensionName.Geometry: {
                        var list = context.ExtensionGeometry
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          var model = list [0];
                          model.Change (action.ModelAction.ExtensionGeometryModel);
                          context.ExtensionGeometry.Update (model);  // change model
                        }
                      }
                      break;

                    case TComponentExtensionName.Image: {
                        var list = context.ExtensionImage
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          var model = list [0];
                          model.Change (action.ModelAction.ExtensionImageModel);
                          context.ExtensionImage.Update (model);  // change model
                        }
                      }
                      break;

                    case TComponentExtensionName.Layout: {
                        var list = context.ExtensionLayout
                          .Where (p => p.Id.Equals (id))
                          .ToList ()
                        ;

                        if (list.Count.Equals (1)) {
                          var model = list [0];
                          model.Change (action.ModelAction.ExtensionLayoutModel);
                          context.ExtensionLayout.Update (model); // change model
                        }
                      }
                      break;

                    case TComponentExtensionName.Node: {
                        // remove old first (use ParentId)
                        var nodeList = context.ExtensionNode
                          .Where (p => p.ParentId.Equals (id))
                          .ToList ()
                        ;

                        foreach (var node in nodeList) {
                          // status
                          var list = context.ComponentStatus
                            .Where (p => p.Id.Equals (node.ChildId))
                            .ToList ()
                          ;

                          // found
                          if (list.Count.Equals (1)) {
                            var model = list [0];
                            model.Busy = false;
                            context.ComponentStatus.Update (model);
                          }

                          // remove
                          context.ExtensionNode.Remove (node);
                        }

                        // next insert new
                        foreach (var node in action.CollectionAction.ExtensionNodeCollection) {
                          // validate same parent
                          if (node.ParentId.Equals (id)) {
                            context.ExtensionNode.Add (node);
                          }
                        }

                        context.SaveChanges (); // update

                        // update status
                        nodeList = context.ExtensionNode
                          .Where (p => p.ParentId.Equals (id))
                          .ToList ()
                        ;

                        foreach (var node in nodeList) {
                          var list = context.ComponentStatus
                            .Where (p => p.Id.Equals (node.ChildId))
                            .ToList ()
                          ;

                          // found
                          if (list.Count.Equals (1)) {
                            var model = list [0];
                            model.Busy = true;
                            context.ComponentStatus.Update (model);
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
                          var model = list [0];
                          model.Change (action.ModelAction.ExtensionTextModel);
                          context.ExtensionText.Update (model); // change model
                        }
                      }
                      break;
                  }
                }
              }

              context.SaveChanges (); // update

              action.Result = TValidationResult.Success;
            }
          }
        }

        catch (Exception exception) {
          Server.Models.Infrastructure.THelper.FormatException ("Change - Full", exception, action);
        }
      }
    }

    void ChangeActive (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
      DATA IN
      - action.Id (Component id to change)
      - action.ModelAction (Component model)
      */

      var id = action.Id;

      try {
        //Component Id must exist
        if (id.IsEmpty ()) {
          action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Change Active] Component Id can NOT be NULL or EMPTY!");
        }

        else {
          // search Id
          var descriptors = context.ComponentDescriptor
            .Where (p => p.Id.Equals (id))
            .ToList ()
          ;

          // Descriptor found
          if (descriptors.Count.Equals (1)) {
            var descriptor = descriptors [0];
            var categoryValue = descriptor.Category;

            // only one component has active status as true
            // reset old
            var statusList = context.ComponentStatus
              .Where (p => p.Active.Equals (true))
              .ToList ()
            ;

            if (statusList.Count.Equals (1)) {
              var model = statusList [0];
              model.Active = false; // reset old
              context.ComponentStatus.Update (model);// change Status model
            }

            // status collection
            foreach (var item in action.CollectionAction.ComponentStatusCollection) {
              var list = context.ComponentStatus
                .Where (p => p.Active.Equals (true))
                .ToList ()
              ;

              // Status found
              if (list.Count.Equals (1)) {
                var model = list [0];
                model.Active = false; // reset old
                context.ComponentStatus.Update (model);// change Status model

                break;
              }
            }

            // new Status 
            statusList = context.ComponentStatus
              .Where (p => p.Id.Equals (id))
              .ToList ()
            ;

            // Status found
            if (statusList.Count.Equals (1)) {
              var model = statusList [0];
              model.Change (action.ModelAction.ComponentStatusModel);
              context.ComponentStatus.Update (model);// change Status model
            }

            // status collection
            foreach (var item in action.CollectionAction.ComponentStatusCollection) {
              var list = context.ComponentStatus
                .Where (p => p.Id.Equals (item.Id))
                .ToList ()
              ;

              // Status found
              if (list.Count.Equals (1)) {
                var model = list [0];
                model.Change (item);
                context.ComponentStatus.Update (model);// change Status model

                break;
              }
            }

            context.SaveChanges (); // update

            action.Result = TValidationResult.Success;
          }
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Change - Active", exception, action);
      }
      
    }

    bool ValidateString (Server.Models.Component.TEntityAction action)
    {
      if (string.IsNullOrEmpty (action.ModelAction.ComponentInfoModel.Name.Trim ())) {
        action.Result = new TValidationResult ($"[{action.Operation.CategoryType.Category} - Change] Name can NOT be NULL or EMPTY!");
        return (false);
      }

      return (true);
    }
    #endregion
  };
  //---------------------------//

}  // namespace