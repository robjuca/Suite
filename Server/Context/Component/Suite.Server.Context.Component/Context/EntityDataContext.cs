/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Server.Context.Component
{
  public sealed class TEntityDataContext : Server.Models.Infrastructure.TEntityDataContextBase
  {
    #region Constructor
    public TEntityDataContext ()
      : base()
    {
      AddOperation (Server.Models.Infrastructure.TOperation.Collection, new TOperationCollection ());
      AddOperation (Server.Models.Infrastructure.TOperation.Insert, new TOperationInsert ());
      AddOperation (Server.Models.Infrastructure.TOperation.Change, new TOperationChange ());
      AddOperation (Server.Models.Infrastructure.TOperation.Remove, new TOperationRemove ());
      AddOperation (Server.Models.Infrastructure.TOperation.Select, new TOperationSelect ());
    }
    #endregion

    #region Overrides
    public override Server.Models.Infrastructure.IModelContext Request (string connectionString)
    {
      return (new TModelContext (connectionString));
    } 
    #endregion
  };
  //---------------------------//

}  // namespace