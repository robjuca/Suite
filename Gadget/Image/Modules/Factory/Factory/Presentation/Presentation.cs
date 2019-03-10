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

namespace Gadget.Factory.Presentation
{
  [Export (typeof (IFactoryPresentation))]
  public class TFactoryPresentation : TPresentation, IFactoryPresentation
  {
    [ImportingConstructor]
    #region Constructor
    public TFactoryPresentation (IEventAggregator events)
      : base (events)
    {
      DelegateCommand = new TPresentationCommand (this);
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