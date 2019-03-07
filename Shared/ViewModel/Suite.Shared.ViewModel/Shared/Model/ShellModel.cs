/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;

using rr.Library.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public abstract class TShellModelReference
  {
    #region Property
    public bool IsViewEnabled
    {
      get;
      set;
    }

    public bool MenuLeftEnabled
    {
      get;
      set;
    }

    public string Message
    {
      get;
      set;
    }

    public string ServiceReportMessage
    {
      get;
      set;
    }

    public bool IsActiveProgress
    {
      get;
      set;
    }

    public int ProgressValue
    {
      get;
      set;
    }

    public Visibility DatabaseOnVisibility
    {
      get;
      private set;
    }

    public Visibility DatabaseOffVisibility
    {
      get;
      private set;
    }

    public Visibility SQLAuthenticationVisibility
    {
      get;
      private set;
    }

    public Visibility WindowsAuthenticationVisibility
    {
      get;
      private set;
    }

    public Visibility LockedVisibility
    {
      get;
      private set;
    }

    public Visibility ServiceReportVisibility
    {
      get;
      private set;
    }

    public TAuthentication Authentication
    {
      get;
      private set;
    }
    public bool IsErrorBoxOpen
    {
      get;
      set;
    }

    public TErrorMessage ErrorBoxContent
    {
      get;
    }
    #endregion

    #region Constructor
    public TShellModelReference ()
    {
      ProgressValue = 0;
      IsViewEnabled = false;

      DatabaseOnVisibility = Visibility.Collapsed;
      DatabaseOffVisibility = Visibility.Visible;

      SQLAuthenticationVisibility = Visibility.Collapsed;
      WindowsAuthenticationVisibility = Visibility.Collapsed;

      LockedVisibility = Visibility.Visible;

      ServiceReportVisibility = Visibility.Collapsed;

      ErrorBoxContent = TErrorMessage.CreateDefault;
    }
    #endregion

    #region Members
    public void ShowErrorBox (TErrorMessage errorMessage)
    {
      if (errorMessage != null) {
        ErrorBoxContent.CopyFrom (errorMessage);
        IsErrorBoxOpen = true;

        if (errorMessage.IsSeverity (TSeverity.Low)) {
          ClearPanels ();
          Unlock ();
        }

        if (errorMessage.IsSeverity (TSeverity.Hight)) {
          ClearPanels ();
          Unlock ();
          DatabaseStatus (false);
        }

        if (errorMessage.IsSeverity (TSeverity.Danger)) {
          ClearPanels ();
          Lock ();
          DatabaseStatus (false);
        }
      }
    }

    public void ShowPanels ()
    {
      IsActiveProgress = true;
      ProgressValue = 80;

      Message = "application is busy...";
    }

    public void ClearPanels ()
    {
      IsActiveProgress = false;
      ProgressValue = 0;

      Message = string.Empty;
    }

    public void ServiceReportShow (string report)
    {
      ServiceReportVisibility = Visibility.Visible;
      ServiceReportMessage = report;
    }

    public void ServiceReportClear ()
    {
      ServiceReportVisibility = Visibility.Collapsed;
      ServiceReportMessage = string.Empty;
    }

    public void Lock ()
    {
      LockedVisibility = Visibility.Visible;
      MenuLeftEnabled = false;
      IsViewEnabled = false;
    }

    public void Unlock ()
    {
      LockedVisibility = Visibility.Collapsed;
      IsViewEnabled = true;
      MenuLeftEnabled = true;
    }

    public void ModalEnter ()
    {
      LockedVisibility = Visibility.Visible;
      IsViewEnabled = false;
    }

    public void ModalLeave ()
    {
      LockedVisibility = Visibility.Collapsed;
      IsViewEnabled = true;
    }

    public void EditEnter ()
    {
      LockedVisibility = Visibility.Visible;
      MenuLeftEnabled = false;
    }

    public void EditLeave ()
    {
      LockedVisibility = Visibility.Collapsed;
      MenuLeftEnabled = true;
    }

    public void DatabaseStatus (bool connected)
    {
      DatabaseOnVisibility = Visibility.Collapsed;
      DatabaseOffVisibility = Visibility.Collapsed;

      SQLAuthenticationVisibility = Visibility.Collapsed;
      WindowsAuthenticationVisibility = Visibility.Collapsed;

      LockedVisibility = Visibility.Collapsed;

      if (connected) {
        IsViewEnabled = true;
        DatabaseOnVisibility = Visibility.Visible;

        switch (Authentication) {
          case TAuthentication.SQL:
            SQLAuthenticationVisibility = Visibility.Visible;
            break;

          case TAuthentication.Windows:
            WindowsAuthenticationVisibility = Visibility.Visible;
            break;
        }
      }

      else {
        DatabaseOffVisibility = Visibility.Visible;
        LockedVisibility = Visibility.Visible;
      }
    }

    public void Select (TAuthentication authentication)
    {
      Authentication = authentication;
    }

    public void MenuLeftEnable ()
    {
      MenuLeftEnabled = true;
    }

    public void MenuLeftDisable ()
    {
      MenuLeftEnabled = false;
    }
    #endregion
  };
  //---------------------------//

}  // namespace