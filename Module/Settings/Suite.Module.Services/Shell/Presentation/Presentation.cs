/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using Caliburn.Micro;

using rr.Library.Infrastructure;

using Shared.Resources;
using Shared.Message;
using Shared.ViewModel;
//---------------------------//

namespace Module.Settings.Shell.Presentation
{
  [Export (typeof (IShellPresentation))]
  public class TShellPresentation : TPresentation, IHandleMessageModule, IShellPresentation
  {
    #region Constructor
    [ImportingConstructor]
    public TShellPresentation (IEventAggregator events)
      : base (events)
    {
      DelegateCommand = new TPresentationCommand (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      if (ViewModel != null) {
        if (message.IsModule (TResource.TModule.Shell) == false) {
          ((IShellViewModel) ViewModel).Message (message);
        }
      }
    } 
    #endregion

    #region Presentation Command
    internal void PublishModuleMessageHandler (TMessageModule message)
    {
      PublishInvoke (message);
    }
    #endregion
  };
  //---------------------------//

}  // namespace