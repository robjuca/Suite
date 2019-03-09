﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Infrastructure;
using rr.Library.Types;

using Shared.Message;

using Shared.Gadget.Image;
//---------------------------//

namespace Gadget.Image.Shell.Presentation
{
  public interface IDelegateCommand : IPresentationCommand
  {
    DelegateCommand<TMessageModule> PublishModuleMessage
    {
      get;
    }

    #region Notify
    DelegateCommand<TNavigateRequestMessage> NotifyNavigateRequestMessage
    {
      get;
    }
    #endregion        
  };
  //---------------------------//

}  // namespace