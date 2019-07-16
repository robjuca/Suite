/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Threading.Tasks;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TEntityService<M>
    where M : IEntityDataContext
  {
    #region Property
    public M DataContext
    {
      get; 
    }
    #endregion

    #region Constructor
    public TEntityService (M dataContext)
    {
      DataContext = dataContext;
    }
    #endregion

    #region Interface
    public async Task<IEntityAction> OperationAsync (IEntityAction entityAction)
    {
      return (await DataContext.OperationAsync (entityAction));
    }
    #endregion
  };
  //---------------------------//

}  // namespace