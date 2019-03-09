/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using Caliburn.Micro;

using rr.Library.Infrastructure;

using Shared.Message;
using Shared.ViewModel;
//---------------------------//

namespace Module.Factory.Presentation
{
  [Export (typeof (IFactoryPresentation))]
  public class TFactoryPresentation : TPresentation, IHandleNavigateResponse, IFactoryPresentation
  {
    [ImportingConstructor]
    #region Constructor
    public TFactoryPresentation (IEventAggregator events)
      : base (events)
    {
      DelegateCommand = new TPresentationCommand (this);
    }
    #endregion

    #region IHandle
    public void Handle (TNavigateResponseMessage message)
    {
      if (message.IsActionNavigateTo) {
        if (message.IsSender (TNavigateMessage.TSender.Shell)) {
          if (message.IsWhere (TNavigateMessage.TWhere.Factory)) {
            ((Pattern.ViewModels.IFactoryViewModel) ViewModel).FocusEnter ();
          }
        }
      }
    }
    #endregion

    #region Presentation Command
    internal void PublishMessageHandler (TMessageModule message)
    {
      PublishInvoke (message);
    }

    internal void PublishInternalMessageHandler (TMessageInternal message)
    {
      PublishInvoke (message);
    }
    #endregion
  };
  //---------------------------//

}  // namespace