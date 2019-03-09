/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Collections.Generic;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Component;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Module.Factory.Presentation;
using Module.Factory.Pattern.Models;
//---------------------------//

namespace Module.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryListViewModel", typeof (IFactoryListViewModel))]
  public class TFactoryListViewModel : TViewModelAware<TFactoryListModel>, IHandleMessageInternal, IFactoryListViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryListViewModel (IFactoryPresentation presentation)
      : base (new TFactoryListModel ())
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
        if (message.Node.IsParentToMe (TChild.List)) {
          // Response
          if (message.IsAction (TInternalMessageAction.Response)) {
            // Collection
            if (message.Support.Argument.Types.IsOperation (Server.Models.Infrastructure.TOperation.Collection)) {
              if (message.Result.IsValid) {
                var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
                Model.SelectComponentSelector (action);

                RefreshCollection ();
              }
            }
          }
        }

        // from sibiling
        if (message.Node.IsSibilingToMe (TChild.List)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            var propertyName = message.Support.Argument.Args.PropertyName;
            var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);

            if (propertyName.Equals ("StyleProperty")) {
              Model.StyleChanged (action);
            }

            if (propertyName.Equals ("all")) {
              Model.StyleChanged (action);
              Model.SelectModel (action);
            }

            RaiseChanged ();
          }

          // Select
          if (message.IsAction (TInternalMessageAction.Select)) {
            //TDispatcher.BeginInvoke (SelectDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            Model.Cleanup ();
            RefreshCollection ();
          }

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
        }
      }
    }
    #endregion

    #region Event
    void OnModelPropertyChanged (object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName.Equals ("CategoryChanged")) {
        var category = Model.RequestComponentSelector ();
        var list = new List<TComponentSourceInfo> ();

        Model.RequestSelectedComponents (list);

        foreach (var info in list) {
          // to sibiling
          var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
          message.Support.Argument.Types.Select (category);
          message.Support.Argument.Args.Select (info.Model, null);

          DelegateCommand.PublishInternalMessage.Execute (message);
        }
      }      
    }

    public void OnComponentSelectorChanged ()
    {
      TDispatcher.Invoke (ComponentSelectorChangedDispatcher);
    }

    public void OnComponentDocumentChecked (object obj)
    {
      if (obj is TComponentSourceInfo info) {
        Model.ComponentSelected (info);
        RefreshCollection ();

        // to sibiling
        var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (Server.Models.Infrastructure.TCategory.Document);
        message.Support.Argument.Args.Select (info.Model, null);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    public void OnComponentImageChecked (object obj)
    {
      if (obj is TComponentSourceInfo info) {
        // to sibiling
        var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Select, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (Server.Models.Infrastructure.TCategory.Image);
        message.Support.Argument.Args.Select (info.Model, null);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    public void OnComponentImageUnchecked (object obj)
    {
      if (obj is TComponentSourceInfo info) {
        // to sibiling
        var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Remove, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (Server.Models.Infrastructure.TCategory.Image);
        message.Support.Argument.Args.Select (info.Model, null);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }

    public void OnComponentDocumentCanRemoveClicked (object obj)
    {
      if (obj is TComponentSourceInfo info) {
        Model.CanRemove (info);
        RefreshCollection ();

        // to sibiling
        var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Modify, TChild.List, TypeInfo);
        message.Support.Argument.Types.Select (Server.Models.Infrastructure.TCategory.Document);

        DelegateCommand.PublishInternalMessage.Execute (message);
      }
    }
    #endregion

    #region Dispatcher
    void ComponentSelectorChangedDispatcher ()
    {
      Server.Models.Infrastructure.TCategory category = Model.RequestComponentSelector ();

      // to sibiling
      var message = new TFactorySibilingMessageInternal (TInternalMessageAction.Modify, TChild.List, TypeInfo);
      message.Support.Argument.Types.Select (category);

      DelegateCommand.PublishInternalMessage.Execute (message);

      // cleanup
      if (category.Equals (Server.Models.Infrastructure.TCategory.None)) {
        RefreshCollection ();
      }

      // request collection
      else {
        var action = TEntityAction.Create (category, Server.Models.Infrastructure.TOperation.Collection, Server.Models.Infrastructure.TExtension.Full);

        // to parent
        var msg = new TFactoryMessageInternal (TInternalMessageAction.Request, TChild.List, TypeInfo);
        msg.Support.Argument.Types.Select (action);

        DelegateCommand.PublishInternalMessage.Execute (msg);
      }
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

    #region Support
    void RefreshCollection ()
    {
      RefreshCollection ("ComponentModelViewSource");
      RaiseChanged ();
    } 
    #endregion
  };
  //---------------------------//

}  // namespace