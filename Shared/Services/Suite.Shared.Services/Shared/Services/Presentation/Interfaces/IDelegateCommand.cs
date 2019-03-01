/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Infrastructure;
using rr.Library.Types;
//---------------------------//

namespace Shared.Services.Presentation
{
  public interface IDelegateCommand : IPresentationCommand
  {
    DelegateCommand<TErrorMessage> NotifyDatabaseError
    {
      get;
    }
  };
  //---------------------------//

}  // namespace