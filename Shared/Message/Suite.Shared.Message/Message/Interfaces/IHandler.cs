/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Caliburn.Micro;
//---------------------------//

namespace Shared.Message
{
  #region Navigate
  public interface IHandleNavigateRequest : IHandle<TNavigateRequestMessage>
  {
  };
  //---------------------------//

  public interface IHandleNavigateResponse : IHandle<TNavigateResponseMessage>
  {
  };
  //---------------------------// 

  public interface IHandleModuleNavigateRequest : IHandle<TModuleNavigateRequestMessage>
  {
  };
  //---------------------------//

  public interface IHandleModuleNavigateResponse : IHandle<TModuleNavigateResponseMessage>
  {
  };
  //---------------------------//
  #endregion

}  // namespace