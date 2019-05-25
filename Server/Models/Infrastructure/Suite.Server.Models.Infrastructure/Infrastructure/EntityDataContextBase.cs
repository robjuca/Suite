/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TEntityDataContextBase : IEntityDataContext
  {
    #region Constructor
    protected TEntityDataContextBase ()
    {
      Operations = new Dictionary<TOperation, IOperation> ();
    }
    #endregion

    #region Virtual Members
    public virtual IModelContext Request (string connectionString)
    {
      return (null);
    }
    #endregion

    #region Interface
    public Task<IEntityAction> OperationAsync (IEntityAction entityAction)
    {
      return (Task.Factory.StartNew (() =>
      {
        if (entityAction.NotNull ()) {
          IModelContext context = Request (entityAction.ConnectionString);

          if (context.NotNull ()) {
            try {
              if (ContainsOperation (entityAction)) {
                Invoke (context, entityAction);
              }

              else {
                     // operation not found!
                     string msg = $"context: {entityAction.Operation.CategoryType.Category} - operation: {entityAction.Operation.Operation} - NOT FOUND!";
                entityAction.Result.CopyFrom (new rr.Library.Helper.TValidationResult (msg));
              }
            }

            catch (Exception exception) {
              THelper.FormatException ("Operation", exception, entityAction);
            }
          }

          context.DisposeNow ();
        }

        return (entityAction);
      })
      );
    }
    #endregion

    #region Protected Members
    protected bool AddOperation (TOperation keyOperation, IOperation operation)
    {
      bool res = false;

      if (ContainsOperation (keyOperation) == false) {
        Operations.Add (keyOperation, operation);
        res = true;
      }

      return (res);
    }

    protected bool ContainsOperation (TOperation keyOperation)
    {
      return (Operations.ContainsKey (keyOperation));
    }

    protected bool ContainsOperation (IEntityAction entityAction)
    {
      return (Operations.ContainsKey (entityAction.Operation.Operation));
    }

    protected bool Invoke (IModelContext context, IEntityAction entityAction)
    {
      bool res = false;

      if (ContainsOperation (entityAction)) {
        Operations [entityAction.Operation.Operation].Invoke (context, entityAction, entityAction.Operation.Extension);
        res = true;
      }

      return (res);
    }
    #endregion

    #region Property
    protected Dictionary<TOperation, IOperation> Operations
    {
      get;
      private set;
    }
    #endregion
  }
  //---------------------------//

}  // namespace