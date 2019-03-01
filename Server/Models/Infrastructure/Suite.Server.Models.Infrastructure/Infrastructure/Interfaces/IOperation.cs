/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Server.Models.Infrastructure
{
  public interface IOperation
  {
    void          Invoke (IModelContext modelContext, IEntityAction entityAction, TExtension extension);
  };
  //---------------------------//

}  // namespace