/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Server.Services.Component
{
  public sealed class TEntityServiceAsync : Server.Models.Infrastructure.TEntityService<Server.Models.Infrastructure.IEntityDataContext>
  {
    #region Constructor
    public TEntityServiceAsync ()
      : base (new Context.Component.TEntityDataContext ())
    {
    }
    #endregion
  };
  //---------------------------//

}  // namespace