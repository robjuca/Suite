/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;
using System.Threading;

using rr.Library.Types;
using rr.Library.Helper;
using rr.Library.Message;

using Shared.Types;
using Shared.Resources;
using Shared.Message;
using Shared.ViewModel;

using Module.Settings.Shell.Presentation;
using Module.Settings.Shell.Pattern.Models;
//---------------------------//

namespace Module.Settings.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TShellViewModel<TShellModel>, IShellViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (new TShellModel (), TProcess.SETTINGS)
    {
      presentation.ViewModel = this;
    }
    #endregion

    #region Overrides
    public override void ProcessMessage (TMessageModule message)
    {
      // services
      if (message.IsModule (TResource.TModule.Services)) {
        SelectAuthentication (message.Support.Argument.Types.Authentication);

        // SettingsValidated
        if (message.IsAction (TMessageAction.SettingsValidated)) {
          // Success
          if (message.Support.IsActionStatus (TActionStatus.Success)) {
            TDispatcher.Invoke (DatabaseSettingsSuccessDispatcher);
          }

          // Error
          if (message.Support.IsActionStatus (TActionStatus.Error)) {
            TDispatcher.Invoke (DatabaseSettingsErrorDispatcher);
          }
        }
      }

      // edit
      if (message.IsModule (TResource.TModule.Edit)) {
        // Changed
        if (message.IsAction (TMessageAction.Changed)) {
          Model.Lock ();
          Model.ShowPanels ();
          Model.SnackbarContent.SetMessage ("settings validating...");
          RaiseChanged ();

          TDispatcher.Invoke (OpenSnackbarDispatcher);
          TDispatcher.BeginInvoke (SaveSettingsDispatcher, message.Support.Argument.Types.ConnectionData);
        }

        // Authentication
        if (message.IsAction (TMessageAction.Authentication)) {
          Model.Lock ();
          Model.ShowPanels ();
          Model.SnackbarContent.SetMessage ("settings validating...");
          RaiseChanged ();

          TDispatcher.Invoke (OpenSnackbarDispatcher);
          TDispatcher.BeginInvoke (ChangeAuthenticationDispatcher, message.Support.Argument.Types.Authentication);
        }
      }
    }
    #endregion

    #region Dispatcher
    void ShowSnackbarDispatcher ()
    {
      System.Threading.Tasks.Task.Factory.StartNew (() =>
      {
        Thread.Sleep (500);
      }).ContinueWith (t =>
      {
        if (FrameworkElementView.FindName ("MainSnackbar") is MaterialDesignThemes.Wpf.Snackbar bar) {
          bar.MessageQueue.Enqueue (Model.SnackbarContent.Message);
        }

        TDispatcher.Invoke (ShutdowDispatcher);

      }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext ());
    }

    void OpenSnackbarDispatcher ()
    {
      if (FrameworkElementView.FindName ("SnackbarActive") is System.Windows.Controls.CheckBox box) {
        box.IsChecked = true;
      }
    }

    void CloseSnackbarDispatcher ()
    {
      if (FrameworkElementView.FindName ("SnackbarActive") is System.Windows.Controls.CheckBox box) {
        box.IsChecked = false;
      }
    }

    void LoadSettingsDispatcher ()
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      DatabaseConnection = DatabaseConnection ?? new TDatabaseConnection (filePath, fileName);

      if (DatabaseConnection.Request ()) {
        // notify edit
        // SQL
        var message = new TShellMessage (TMessageAction.Response, TypeInfo);
        message.Support.Argument.Types.ConnectionData.CopyFrom (DatabaseConnection.Request (TAuthentication.SQL));

        DelegateCommand.PublishModuleMessage.Execute (message);

        // Windows
        message = new TShellMessage (TMessageAction.Response, TypeInfo);
        message.Support.Argument.Types.ConnectionData.CopyFrom (DatabaseConnection.Request (TAuthentication.Windows));

        DelegateCommand.PublishModuleMessage.Execute (message);

        // settings validating
        TDispatcher.Invoke (RequestServiceValidationDispatcher);
      }

      else {
        var errorMessage = new TErrorMessage ("Settings ERROR", "Load Settings Dispatcher", (string) DatabaseConnection.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      }
    }

    void SaveSettingsDispatcher (TDatabaseAuthentication databaseAuthentication)
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      DatabaseConnection = DatabaseConnection ?? new TDatabaseConnection (filePath, fileName);

      if (DatabaseConnection.Save (databaseAuthentication)) {
        DatabaseConnection.Request (); // update

        // notify edit
        var message = new TShellMessage (TMessageAction.Response, TypeInfo);
        message.Support.Argument.Types.ConnectionData.CopyFrom (databaseAuthentication);

        DelegateCommand.PublishModuleMessage.Execute (message);

        // settings validating
        TDispatcher.Invoke (RequestServiceValidationDispatcher);
      }

      else {
        TDispatcher.Invoke (CloseSnackbarDispatcher);

        var errorMessage = new TErrorMessage ("Settings ERROR", "Save Settings Dispatcher", (string) DatabaseConnection.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      }


      //else {
      //  var errorMessage = new TErrorMessage ("Save Settings Dispatcher", "Shell Settings", "DatabaseData is NULL")
      //  {
      //    Severity = TSeverity.Hight
      //  };

      //  TDispatcher.BeginInvoke (ShowErrorBoxDispatcher, errorMessage);
      //}
    }

    void ChangeAuthenticationDispatcher (TAuthentication authentication)
    {
      DatabaseConnection.ChangeAuthentication (authentication);

      TDispatcher.Invoke (LoadSettingsDispatcher);
    }

    void RequestServiceValidationDispatcher ()
    {
      // settings validating
      var message = new TShellMessage (TMessageAction.SettingsValidating, TypeInfo);
      message.Support.Argument.Types.ConnectionData.CopyFrom (DatabaseConnection.CurrentDatabase);

      DelegateCommand.PublishModuleMessage.Execute (message);
    }

    void DatabaseSettingsSuccessDispatcher ()
    {
      // notify main process
      NotifyMainProcess ("success");

      // update INI file
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var data = new TDatabaseConnection (filePath, fileName);
      data.SelectValidate (true);

      Model.ClearPanels ();
      Model.DatabaseStatus (true);
      Model.Unlock ();

      RaiseChanged ();

      TDispatcher.Invoke (CloseSnackbarDispatcher);

      Model.SnackbarContent.SetMessage ("Welcome to Suite 18 application");
      TDispatcher.Invoke (ShowSnackbarDispatcher);
    }

    void DatabaseSettingsErrorDispatcher ()
    {
      // notify main process
      NotifyMainProcess ("error");

      TDispatcher.Invoke (CloseSnackbarDispatcher);
    }

    void ShutdowDispatcher ()
    {
      if (Properties.Settings.Default.Shutdown) {
        Properties.Settings.Default.Shutdown = false;
        Properties.Settings.Default.Save ();

        System.Threading.Tasks.Task.Factory.StartNew (() =>
        {
          Thread.Sleep (4500);
        }).ContinueWith (t =>
        {
          NotifyMainProcess ("shutdown");

          (FrameworkElementView as System.Windows.Window).Close ();
        }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext ());
      }
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      Model.Lock ();
      Model.ShowPanels ();
      Model.SnackbarContent.SetMessage ("settings validating...");
      RaiseChanged ();

      TDispatcher.Invoke (OpenSnackbarDispatcher);
      TDispatcher.Invoke (LoadSettingsDispatcher);
    }
    #endregion

    #region Property
    IDelegateCommand DelegateCommand
    {
      get
      {
        return (PresentationCommand as IDelegateCommand);
      }
    }

    TDatabaseConnection DatabaseConnection
    {
      get;
      set;
    }
    #endregion

    #region Fields
    
    #endregion
  };
  //---------------------------//

}  // namespace