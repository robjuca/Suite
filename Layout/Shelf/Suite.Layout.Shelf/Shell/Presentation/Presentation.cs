/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using Caliburn.Micro;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Shared.Message;
using Shared.Resources;
using Shared.ViewModel;

using Layout.Shelf.Shell.Pattern.ViewModels;
//---------------------------//

namespace Layout.Shelf.Shell.Presentation
{
  [Export (typeof (IShellPresentation))]
  public class TShellPresentation : TPresentation, IHandleNavigateRequest, IHandleMessageModule, IShellPresentation
  {
    #region Constructor
    [ImportingConstructor]
    public TShellPresentation (IEventAggregator events)
      : base (events)
    {
      DelegateCommand = new TPresentationCommand (this);
    }
    #endregion

    #region IHandle Members
    public void Handle (TMessageModule message)
    {
      if (ViewModel.NotNull ()) {
        if (message.IsModule (TResource.TModule.Shell).IsFalse ()) {
          ((IShellViewModel) ViewModel).Message (message);
        }
      }
    }

    public void Handle (TNavigateRequestMessage message)
    {
      if (message.IsActionRequest) {
        if (message.Sender.Equals (TNavigateMessage.TSender.Shell)) {
          m_NavigateRequestMessage = message;

          THelper.DispatcherLater (NavigateRequestDispatcher);
        }
      }
    }
    #endregion

    #region Presentation Command
    internal void PublishModuleMessageHandler (TMessageModule message)
    {
      PublishInvoke (message);
    }

    internal void NotifyNavigateRequestMessageHandler (TNavigateRequestMessage message)
    {
      Publish (message);
    }
    #endregion

    #region Dispatcher
    void NavigateRequestDispatcher ()
    {
      Type typeNavigateTo = null;

      switch (m_NavigateRequestMessage.Where) {
        case TNavigateMessage.TWhere.Collection:
          typeNavigateTo = typeof (TShellCollectionViewModel);
          break;

        case TNavigateMessage.TWhere.Factory:
          typeNavigateTo = typeof (TShellFactoryViewModel);
          break;
      }

      if (typeNavigateTo.NotNull ()) {
        var message = new TNavigateResponseMessage (m_NavigateRequestMessage.Sender, m_NavigateRequestMessage.Where, (Type) typeNavigateTo);

        Publish (message);
      }
    }
    #endregion

    #region Fields
    TNavigateRequestMessage                                               m_NavigateRequestMessage;
    #endregion
  };
  //---------------------------//

}  // namespace