/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Infrastructure;
using rr.Library.Types;

using Shared.Message;
using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Factory.Database.Presentation
{
  public interface IDelegateCommand : IPresentationCommand
  {
    DelegateCommand<TMessageModule> PublishMessage
    {
      get;
    }

    DelegateCommand<TMessageInternal> PublishInternalMessage
    {
      get;
    }
  };
  //---------------------------//

}  // namespace