/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Server.Models.Infrastructure
{
  public abstract class TEntityAction<M, C> : TEntityActionBase<TCategoryType>
  {
    #region Property
    public M ModelAction
    {
      get;
      private set;
    }

    public C CollectionAction
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TEntityAction (M model, C collection, TCategory category, string connectionString)
      : base (TCategoryType.Create (category), connectionString)
    {
      ModelAction = model;
      CollectionAction = collection;
    }

    public TEntityAction (M model, C collection, TCategory category, string connectionString, object param1, object param2)
      : base (TCategoryType.Create (category), connectionString, param1, param2)
    {
      ModelAction = model;
      CollectionAction = collection;
    }
    #endregion
  };
  //---------------------------//

}  // namespace