/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.ViewModel
{
  public interface IShellViewModel : rr.Library.Infrastructure.IViewModel
  {
    void          Message (Shared.Message.TMessageModule message);
  };
  //---------------------------//

}  // namespace