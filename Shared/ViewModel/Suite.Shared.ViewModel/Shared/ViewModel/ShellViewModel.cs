﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using XDMessaging;

using rr.Library.Helper;
using rr.Library.Infrastructure;
using rr.Library.Types;

using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public class TShellViewModel<M> : TViewModelAware<M>, IShellViewModel
    where M : TShellModelReference
  {
    #region Property
    public string ProcessName
    {
      get;
    } 
    #endregion

    #region Constructor
    public TShellViewModel (M model, string processName)
      : base (model)
    {
      ProcessName = processName;

      TypeName = GetType ().Name;

      m_ModalCount = 0;

      m_MessagingClient = new XDMessagingClient ();
      m_Broadcaster = m_MessagingClient.Broadcasters.GetBroadcasterForMode (XDTransportMode.HighPerformanceUI);
    }
    #endregion

    #region Interface Members
    public void Message (Shared.Message.TMessageModule message)
    {
      // error
      if (message.IsAction (TMessageAction.Error)) {
        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, message.Support.ErrorMessage);
      }

      // modal enter
      if (message.IsAction (TMessageAction.ModalEnter)) {
        if (m_ModalCount.Equals (0)) {
          Model.ModalEnter ();
          Model.ShowPanels ();

          RaiseChanged ();
        }

        m_ModalCount++;
      }

      // modal leave
      if (message.IsAction (TMessageAction.ModalLeave)) {
        if (m_ModalCount > 0) {
          m_ModalCount--;

          if (m_ModalCount.Equals (0)) {
            Model.ModalLeave ();
            Model.ClearPanels ();

            RaiseChanged ();
          }
        }
      }

      // edit enter
      if (message.IsAction (TMessageAction.EditEnter)) {
        Model.EditEnter ();
        RaiseChanged ();
      }

      // edit leave
      if (message.IsAction (TMessageAction.EditLeave)) {
        Model.EditLeave ();
        RaiseChanged ();
      }

      // show service report
      if (message.IsAction (TMessageAction.ReportShow)) {
        Model.ServiceReportShow (message.Support.Argument.Types.ReportData.Message);
        RaiseChanged ();
      }

      // clear service report
      if (message.IsAction (TMessageAction.ReportClear)) {
        Model.ServiceReportClear ();
        RaiseChanged ();
      }

      ProcessMessage (message);
    }

    public void SelectAuthentication (TAuthentication authentication)
    {
      Model.Select (authentication);
    }

    public void NotifyMainProcess (string message)
    {
      m_Broadcaster.SendToChannel (ProcessName, message);
    }
    #endregion

    #region Virtal Members
    public virtual void ProcessMessage (Shared.Message.TMessageModule message)
    {
    }
    #endregion

    #region Event
    void OnClosing (object sender, System.ComponentModel.CancelEventArgs e)
    {
      NotifyMainProcess ("closed");
    }
    #endregion

    #region Dispatcher
    public void ShowErrorBoxDispatcher (TErrorMessage errorMessage)
    {
      Model.ShowErrorBox (errorMessage);

      RaiseChanged ();
    }
    #endregion

    #region Overrides
    protected override void AllDone ()
    {
      (FrameworkElementView as System.Windows.Window).Closing += OnClosing;
    } 
    #endregion

    #region Fields
    XDMessagingClient                       m_MessagingClient;
    IXDBroadcaster                          m_Broadcaster;
    int                                     m_ModalCount;
    #endregion
  };
  //---------------------------//

}  // namespace