/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;

using Shared.Message;
//---------------------------//

namespace Module.Settings.Shell.Presentation
{
  public class TPresentationCommand : IDelegateCommand
  {
    #region IDelegateCommand Members
    public DelegateCommand<TMessageModule> PublishModuleMessage
    {
      get;
      private set;
    }

    #region Notify
    public DelegateCommand<TNavigateRequestMessage> NotifyNavigateRequestMessage
    {
      get;
      private set;
    }
    #endregion        
    #endregion

    #region Constructor
    public TPresentationCommand (TShellPresentation presentation)
    {
      PublishModuleMessage = new DelegateCommand<TMessageModule> (presentation.PublishModuleMessageHandler);

      NotifyNavigateRequestMessage = new DelegateCommand<TNavigateRequestMessage> (presentation.NotifyNavigateRequestMessageHandler);
    }
    #endregion
  }
  //---------------------------//

}  // namespace