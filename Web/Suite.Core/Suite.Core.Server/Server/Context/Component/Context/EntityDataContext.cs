/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Linq;

using rr.Library.Helper;
//---------------------------//

namespace Server.Context.Component
{
  public static class TEntityDataContext 
  {
    #region Members
    public static void SelectActive (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      if (context.NotNull ()) {
        if (action.NotNull ()) {
          try {

            var relationList = context.CategoryRelation
              .ToList ()
            ;

            action.CollectionAction.SetCollection (relationList);

            // Active status
            var statusList = context.ComponentStatus
              .Where (p => p.Active.Equals (true))
              .ToList ()
            ;

            // status found
            if (statusList.Count.Equals (1)) {
              var statusModel = statusList [0];
              action.Id = statusModel.Id;
              action.CollectionAction.ComponentStatusCollection.Add (statusModel);

              action.Result = TValidationResult.Success; // desired result DO NOT MOVE FROM HERE

              SelectById (context, action);
            }
          }

          catch (Exception exception) {
            Server.Models.Infrastructure.THelper.FormatException ("Select Active", exception, action);
          }
        }
      }
    }
    #endregion

    static void SelectById (TModelContext context, Server.Models.Component.TEntityAction action)
    {
      /*
      DATA IN
      - action.Id 
      - action.CollectionAction.CategoryRelationCollection

      DATA OUT
      - action.ModelAction (model)
      - action.CollectionAction.ModeCollection {id, model} (for each node)
      */

      try {
        // Id must exist
        if (action.Id.IsEmpty ()) {
          action.Result = new TValidationResult ("[Select ById] Id can NOT be NULL or EMPTY!");
        }

        else {
          action.Result = TValidationResult.Success; // desired result DO NOT MOVE FROM HERE

          // relation by id (use parent)
          action.CollectionAction.SelectComponentOperation (Server.Models.Component.TComponentOperation.TInternalOperation.Id);
          action.ComponentOperation.SelectById (action.Id);

          var operationSupport = new TOperationSupport (context, action);
          operationSupport.RequestComponent (context, action);
          operationSupport.RequestExtension (context, action);
          operationSupport.RequestNode (context, action);
          operationSupport.RequestRelation (context, action);

          action.Param1 = operationSupport.CategoryValue;

          // use Parent relation
          if (action.ComponentOperation.ParentIdCollection.ContainsKey (action.Id)) {
            var componentRelationList = action.ComponentOperation.ParentIdCollection [action.Id];

            foreach (var relation in componentRelationList) {
              var entityAction = Server.Models.Component.TEntityAction.CreateDefault;
              entityAction.CollectionAction.SetCollection (action.CollectionAction.CategoryRelationCollection);
              entityAction.Id = relation.ChildId;

              SelectById (context, entityAction); // my self (tree navigation)

              action.CollectionAction.EntityCollection.Add (relation.ChildId, entityAction);
            }
          }
        }
      }

      catch (Exception exception) {
        Server.Models.Infrastructure.THelper.FormatException ("Select ById", exception, action);
      }
    }
  };
  //---------------------------//

}  // namespace