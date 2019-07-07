/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Diagnostics;

using rr.Library.Infrastructure;
using rr.Library.Helper;
using rr.Library.Communication;

using Shared.Communication;

using Suite.Launcher.Shell.Presentation;
using Suite.Launcher.Shell.Pattern.Models;
//---------------------------//

namespace Suite.Launcher.Shell.Pattern.ViewModels
{
  [Export (typeof (IShellViewModel))]
  public class TShellViewModel : TViewModelAware<TShellModel>, IShellViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TShellViewModel (IShellPresentation presentation)
      : base (new TShellModel ())
    {
      TypeName = GetType ().Name;

      presentation.ViewModel = this;

      m_Process = new Dictionary<string, Process> ();
      m_Modules = new Dictionary<string, string> ();

      string [] keys = new string []
        {
          "Gadget.Document",
          "Gadget.Image",
          "Layout.Bag",
          "Layout.Shelf",
          "Layout.Drawer",
          "Layout.Chest",
          "Module.Settings",
        };

      for (int i = 0; i < Modules.Length; i++) {
        m_Modules.Add (Modules [i], keys [i]);
      }

      m_CurrentModule = TProcessName.Settings;
      m_SettingsValidating = true;

      m_DataComm = TDataComm.CreateDefault;

      m_Communication = new TMessagingComm<TDataComm> (m_DataComm);
      m_Communication.Handle += OnCommunicationHandle; // Attach event handler for incoming messages
    }
    #endregion

    #region View Event
    public void OnDocumentCommadClicked ()
    {
      m_CurrentModule = TProcessName.Document;
      TDispatcher.BeginInvoke (StartProcessDispatcher, "Suite.Gadget");
    }

    public void OnImageCommadClicked ()
    {
      m_CurrentModule = TProcessName.Image;
      TDispatcher.BeginInvoke (StartProcessDispatcher, "Suite.Gadget");
    }

    public void OnBagCommadClicked ()
    {
      m_CurrentModule = TProcessName.Bag;
      TDispatcher.BeginInvoke( StartProcessDispatcher, "Suite.Layout");
    }

    public void OnShelfCommadClicked ()
    {
      m_CurrentModule = TProcessName.Shelf;
      TDispatcher.BeginInvoke (StartProcessDispatcher, "Suite.Layout");
    }

    public void OnDrawerCommadClicked ()
    {
      m_CurrentModule = TProcessName.Drawer;
      TDispatcher.BeginInvoke (StartProcessDispatcher, "Suite.Layout");
    }

    public void OnChestCommadClicked ()
    {
      m_CurrentModule = TProcessName.Chest;
      TDispatcher.BeginInvoke (StartProcessDispatcher, "Suite.Layout");
    }

    public void OnSettingsCommadClicked ()
    {
      m_CurrentModule = TProcessName.Settings;
      THelper.DispatcherLater (StartSettingsProcessDispatcher);
    }
    #endregion

    #region Dispatcher
    void StartSettingsProcessDispatcher ()
    {
      if (m_CurrentModule.Equals (TProcessName.Settings)) {
        var module = m_CurrentModule.ToString ();
        var key = m_Modules [module];
        var processName = $"Suite.Module.{module}.exe";

        if (m_Process.ContainsKey (key)) {
          if (m_Process [key].HasExited) {
            m_Process [key].Start ();
          }
        }

        else {
          var processKey = key;

          if (m_CurrentModule.Equals (TProcessName.Settings)) {
            if (m_SettingsValidating) {
              processKey += ".Validating";
            }
          }

          Process process = new Process
          {
            StartInfo = new ProcessStartInfo (processName, processKey)
          };

          process.Start ();

          m_Process.Add (key, process);
        }

        Model.DisableAll ();
        RaiseChanged ();
      }
    }

    void StartProcessDispatcher (string processName)
    {
      var module = m_CurrentModule.ToString ();
      var key = m_Modules [module];

      processName += $".{module}.exe";

      if (m_Process.ContainsKey (key)) {
        if (m_Process [key].HasExited) {
          m_Process [key].Start ();
        }
      }

      else {
        var processKey = key;

        Process process = new Process
        {
          StartInfo = new ProcessStartInfo (processName, processKey)
        };

        process.Start ();

        m_Process.Add (key, process);
      }

      Model.MenuOnly ();
      RaiseChanged ();
    }

    void RemoveProcessPartialDispatcher ()
    {
      foreach (var module in Modules) {
        if (module.Equals (TProcessName.Settings.ToString())) {
          continue;
        }

        RemoveProcess (module);
      }
    }
    #endregion

    #region Event
    void OnClosing (object sender, System.ComponentModel.CancelEventArgs e)
    {
      foreach (var process in m_Process) {
        if (process.Value.HasExited == false) {
          process.Value.Kill ();
        }
      }

      m_Process.Clear ();
    }
    #endregion

    #region MessageEvent
    void OnCommunicationHandle (object sender, TMessagingEventArgs<TDataComm> e)
    {
      var module = Enum.Parse (typeof (TProcessName), e.Data.ClientName);

      switch (module) {
        case TProcessName.Settings: {
            switch (e.Data.Command) {
              case TCommandComm.Shutdown: {
                  RemoveProcess (SETTINGS);
                }
                break;

              case TCommandComm.Closed: {
                  RemoveProcess (SETTINGS);
                  Model.EnableAll ();
                  RaiseChanged ();
                }
                break;

              case TCommandComm.Success: {
                  m_SettingsValidating = false;
                }
                break;

              case TCommandComm.Error: {
                  Model.SettingsOnly ();
                  RaiseChanged ();

                  m_SettingsValidating = true;

                  THelper.DispatcherLater (RemoveProcessPartialDispatcher);
                }
                break;
            }
          }
          break;

        case TProcessName.Document: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (DOCUMENT);
                }
                break;
            }
          }
          break;

        case TProcessName.Image: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (IMAGE);
                }
                break;
            }
          }
          break;

        case TProcessName.Bag: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (BAG);
                }
                break;
            }
          }
          break;

        case TProcessName.Shelf: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (SHELF);
                }
                break;
            }
          }
          break;

        case TProcessName.Drawer: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (DRAWER);
                }
                break;
            }
          }
          break;

        case TProcessName.Chest: {
            switch (e.Data.Command) {
              case TCommandComm.Closed: {
                  RemoveProcess (CHEST);
                }
                break;
            }
          }
          break;
      }
    } 
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      (FrameworkElementView as System.Windows.Window).Closing += OnClosing;

      OnSettingsCommadClicked ();
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
    #endregion

    #region Data
    enum TProcessName
    {
      Document,
      Image,
      Bag,
      Shelf,
      Drawer,
      Chest,
      Settings,
    };
    #endregion

    #region Property
    static string DOCUMENT
    {
      get
      {
        return (TProcessName.Document.ToString ());
      }
    }

    static string IMAGE
    {
      get
      {
        return (TProcessName.Image.ToString ());
      }
    }

    static string BAG
    {
      get
      {
        return (TProcessName.Bag.ToString ());
      }
    }

    static string SHELF
    {
      get
      {
        return (TProcessName.Shelf.ToString ());
      }
    }

    static string DRAWER
    {
      get
      {
        return (TProcessName.Drawer.ToString ());
      }
    }

    static string CHEST
    {
      get
      {
        return (TProcessName.Chest.ToString ());
      }
    }

    static string SETTINGS
    {
      get
      {
        return (TProcessName.Settings.ToString ());
      }
    }
    #endregion

    #region Fields
    readonly TMessagingComm<TDataComm>                                    m_Communication;
    readonly TDataComm                                                    m_DataComm;
    readonly Dictionary<string, Process>                                  m_Process;
    readonly Dictionary<string, string>                                   m_Modules;
    TProcessName                                                          m_CurrentModule;
    bool                                                                  m_SettingsValidating;
    static readonly string []                                             Modules = new string [] { DOCUMENT, IMAGE, BAG, SHELF, DRAWER, CHEST, SETTINGS };
    #endregion

    #region Support
    void RemoveProcess (string moduleName)
    {
      // remove process
      var key = m_Modules [moduleName];

      if (m_Process.ContainsKey (key)) {
        var process = m_Process [key];

        if (process.HasExited.IsFalse ()) {
          process.Kill ();
        }

        m_Process.Remove (key);
      }

      if (m_Process.Count.Equals (0)) {
        Model.EnableAll ();
        RaiseChanged ();
      }
    } 
    #endregion
  };
  //---------------------------//

}  // namespace