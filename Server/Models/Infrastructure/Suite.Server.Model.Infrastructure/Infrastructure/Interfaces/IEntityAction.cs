/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Helper;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public interface IEntityAction
  {
    string                                ConnectionString { get; set;}
    TValidationResult                     Result { get; }
    TEntityOperation<TCategoryType>       Operation { get; }
    object                                Param1 { get; set;}
    object                                Param2 { get; set;}
  }
  //---------------------------//

}  // namespace