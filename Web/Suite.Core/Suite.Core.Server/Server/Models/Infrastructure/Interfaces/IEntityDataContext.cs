/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Threading.Tasks;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public interface IEntityDataContext
  {
    Task<IEntityAction>         OperationAsync (IEntityAction entityAction);
  }
  //---------------------------//

}  // namespace