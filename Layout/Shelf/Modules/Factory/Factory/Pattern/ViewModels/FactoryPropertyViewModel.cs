/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Controls;
using rr.Library.Helper;
using rr.Library.Types;

using Server.Models.Component;

using Shared.Resources;
using Shared.Types;
using Shared.ViewModel;

using Layout.Factory.Presentation;
using Layout.Factory.Pattern.Models;
//---------------------------//

namespace Layout.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryPropertyViewModel", typeof (IFactoryPropertyViewModel))]
  public class TFactoryPropertyViewModel : TViewModelAware<TFactoryPropertyModel>, IHandleMessageInternal, IFactoryPropertyViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryPropertyViewModel (IFactoryPresentation presentation)
      : base (new TFactoryPropertyModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);

      Model.PropertyChanged += OnModelPropertyChanged;
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // from parent
        if (message.Node.IsParentToMe (TChild.Property)) {
          // edit
          if (message.IsAction (TInternalMessageAction.Edit)) {
            TDispatcher.BeginInvoke (EditDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            if (message.Result.IsValid) {
              // insert
              if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Insert)) {
                TDispatcher.Invoke (InsertSuccessDispatcher);
              }

              // change - full
              if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Change, Server.Models.Infrastructure.TExtension.Full)) {
                TDispatcher.Invoke (ChangeSuccessDispatcher);
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.Property)) {
          // LockEnter
          if (message.IsAction (TInternalMessageAction.LockEnter)) {
            Model.LockEnter ();
            RaiseChanged ();
          }

          // LockLeave
          if (message.IsAction (TInternalMessageAction.LockLeave)) {
            Model.LockLeave ();
            RaiseChanged ();
          }

          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            TDispatcher.BeginInvoke (ResponseModelDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Report
          if (message.IsAction (TInternalMessageAction.Report)) {
            Model.SelectReport (message.Support.Argument.Types.ReportData);

            TDispatcher.Invoke (RefreshAllDispatcher);
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnPropertyGridComponentLoaded (object control)
    {
      if (control is TPropertyGrid) {
        m_PropertyGridComponent = m_PropertyGridComponent ?? (TPropertyGrid) control;
      }
    }

    public void OnPropertyGridExtensionLoaded (object control)
    {
      if (control is TPropertyGrid) {
        m_PropertyGridExtension = m_PropertyGridExtension ?? (TPropertyGrid) control;
      }
    }

    void OnModelPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      PropertySelect (e.PropertyName);
    }

    public void OnApplyCommadClicked ()
    {
      Model.ShowPanels ();
      RaiseChanged ();

      var action = TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Insert);

      if (IsViewModeEdit) {
        action = TEntityAction.Create (Server.Models.Infrastructure.TCategory.Shelf, Server.Models.Infrastructure.TOperation.Change, Server.Models.Infrastructure.TExtension.Full);
      }

      Model.RequestModel (action);

      TDispatcher.BeginInvoke (RequestModelDispatcher, action);
    }

    public void OnCancelCommadClicked ()
    {
      TDispatcher.Invoke (CleanupDispatcher);
      TDispatcher.Invoke (EditLeaveDispatcher);
    }
    #endregion

    #region Dispatcher
    void RefreshAllDispatcher ()
    {
      RaiseChanged ();

      if (m_PropertyGridComponent.NotNull ()) {
        m_PropertyGridComponent.RefreshPropertyList ();
      }

      if (m_PropertyGridExtension.NotNull ()) {
        m_PropertyGridExtension.RefreshPropertyList ();
      }
    }

    void CleanupDispatcher ()
    {
      Cleanup ();
    }

    void CleanupMySelfDispatcher ()
    {
      CleanupMySelf ();
    }

    void RequestModelDispatcher (TEntityAction action)
    {
      //to sibiling 
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Request, TChild.Property, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void ResponseModelDispatcher (TEntityAction action)
    {
      TDispatcher.BeginInvoke (ApplyDispatcher, action);
    }

    void ApplyDispatcher (TEntityAction action)
    {
      // to parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.Property, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void InsertSuccessDispatcher ()
    {
      TDispatcher.Invoke (CleanupDispatcher);
      TDispatcher.Invoke (ReloadDispatcher);
      TDispatcher.Invoke (RefreshDispatcher);
    }

    void ChangeSuccessDispatcher ()
    {
      TDispatcher.Invoke (CleanupDispatcher);
      TDispatcher.Invoke (EditLeaveDispatcher);
      TDispatcher.Invoke (ReloadDispatcher);
      TDispatcher.Invoke (RefreshDispatcher);
    }

    void ReloadDispatcher ()
    {
      // to parent
      var message = new TFactoryMessageInternal (TInternalMessageAction.Reload, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void RefreshDispatcher ()
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Refresh, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void EditEnterDispatcher ()
    {
      // to parent request focus
      var message = new TFactoryMessageInternal (TInternalMessageAction.NavigateForm, TChild.Property, TypeInfo);
      message.Support.Argument.Args.Select (TWhere.Factory);
      DelegateCommand.PublishInternalMessage.Execute (message);

      // to parent
      var msg = new TFactoryMessageInternal (TInternalMessageAction.EditEnter, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (msg);
    }

    void EditLeaveDispatcher ()
    {
      // to parent leave focus
      var message = new TFactoryMessageInternal (TInternalMessageAction.NavigateForm, TChild.Property, TypeInfo);
      message.Support.Argument.Args.Select (TWhere.Collection);
      DelegateCommand.PublishInternalMessage.Execute (message);

      // to parent  
      var msg = new TFactoryMessageInternal (TInternalMessageAction.EditLeave, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (msg);
    }

    void EditSibilingDispatcher (TEntityAction action)
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Edit, TChild.Property, TypeInfo);
      message.Support.Argument.Types.Select (action);

      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    void EditDispatcher (TEntityAction action)
    {
      var id = action.Id;

      // must exist
      if (id.NotEmpty ()) {
        SelectViewMode (TViewMode.Edit);

        Model.Cleanup ();
        Model.SelectModel (action);

        TDispatcher.Invoke (RefreshAllDispatcher);
        TDispatcher.Invoke (EditEnterDispatcher);

        PropertySelect ("Int4Property");

        TDispatcher.BeginInvoke (EditSibilingDispatcher, action);
      }
    }
    #endregion

    #region Overrides
    protected override void Initialize ()
    {
      Model.Cleanup ();
      Model.Initialize ();

      RaiseChanged ();
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

    #region Fields
    TPropertyGrid                           m_PropertyGridComponent;
    TPropertyGrid                           m_PropertyGridExtension;
    #endregion

    #region Support
    void PropertySelect (string propertyName)
    {
      var action = Server.Models.Component.TEntityAction.CreateDefault;
      Model.ComponentModelProperty.RequestModel (action);

      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.PropertySelect, TChild.Property, TypeInfo);
      message.Support.Argument.Types.Select (action);
      message.Support.Argument.Args.Select (propertyName);

      DelegateCommand.PublishInternalMessage.Execute (message);

      RaiseChanged ();
    }

    void Cleanup ()
    {
      CleanupSibiling ();

      TDispatcher.Invoke (CleanupMySelfDispatcher);
    }

    void CleanupPropertyControl ()
    {
      if (m_PropertyGridComponent.NotNull ()) {
        m_PropertyGridComponent.Cleanup ();
      }

      if (m_PropertyGridExtension.NotNull ()) {
        m_PropertyGridExtension.Cleanup ();
      }
    }

    void CleanupSibiling ()
    {
      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Cleanup, TChild.Property, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);

      PropertySelect ("all");
    }

    void CleanupMySelf ()
    {
      Model.Cleanup ();

      CleanupPropertyControl ();

      Model.Initialize ();

      ResetViewMode ();

      TDispatcher.Invoke (RefreshAllDispatcher);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
               