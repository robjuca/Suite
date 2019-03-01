/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;
//---------------------------//

namespace Shared.Services.Presentation
{
  public class TPresentationCommand : IDelegateCommand
  {
    #region IDelegateCommand Members
    public DelegateCommand<TErrorMessage> NotifyDatabaseError
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TPresentationCommand (TServicesPresentation presentation)
    {
      NotifyDatabaseError = new DelegateCommand<TErrorMessage> (presentation.NotifyDatabaseErrorHandler);
    }
    #endregion
  };
  //---------------------------//

}  // namespace