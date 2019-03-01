/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Services;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public interface IEntityOperation
  {
    void          Operation (TServiceAction<IEntityAction> serviceAction);
  }
  //---------------------------//

}  // namespace