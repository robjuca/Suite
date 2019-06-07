/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition;

using rr.Library.Infrastructure;
using rr.Library.Helper;

using Server.Models.Component;

using Shared.Types;
using Shared.Resources;
using Shared.ViewModel;

using Layout.Factory.Presentation;
using Layout.Factory.Pattern.Models;
//---------------------------//

namespace Layout.Factory.Pattern.ViewModels
{
  [Export ("ModuleFactoryDisplayViewModel", typeof (IFactoryDisplayViewModel))]
  public class TFactoryDisplayViewModel : TViewModelAware<TFactoryDisplayModel>, IHandleMessageInternal, IFactoryDisplayViewModel
  {
    #region Constructor
    [ImportingConstructor]
    public TFactoryDisplayViewModel (IFactoryPresentation presentation)
      : base (new TFactoryDisplayModel ())
    {
      TypeName = GetType ().Name;

      presentation.RequestPresentationCommand (this);
      presentation.EventSubscribe (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageInternal message)
    {
      if (message.IsModule (TResource.TModule.Factory)) {
        // only from Sibling
        if (message.Node.IsSiblingToMe (TChild.Display)) {
          // PropertySelect
          if (message.IsAction (TInternalMessageAction.PropertySelect)) {
            var propertyName = message.Support.Argument.Args.PropertyName;

            if (propertyName.Equals ("StyleProperty")) {
              var action = TEntityAction.Request (message.Support.Argument.Types.EntityAction);
              Model.StyleChanged (action);

              RefreshAll ();
            }
          }

          // Select
          if (message.IsAction (TInternalMessageAction.Select)) {
            if (message.Support.Argument.Args.Param1 is TComponentModelItem model) {
              var category = message.Support.Argument.Types.Category;

              Model.Select (category, model);
              RefreshAll ();
            }
          }

          // Remove
          if (message.IsAction (TInternalMessageAction.Remove)) {
            if (message.Support.Argument.Args.Param1 is TComponentModelItem model) {
              var category = message.Support.Argument.Types.Category;

              Model.Remove (category, model);
              RefreshAll ();
            }
          }

          // Modify
          if (message.IsAction (TInternalMessageAction.Modify)) {
            Model.Modify (message.Support.Argument.Types.Category);
            RefreshAll ();
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestModelDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            Model.Cleanup ();
            RefreshAll ();
          }
        }
      }
    }
    #endregion

    #region Event
    public void OnOrderClicked ()
    {
      Model.RequestOrder ();
      RefreshAll ();

      //to Sibling 
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.LockEnter, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }

    public void OnOrderBackClicked ()
    {
      Model.ReOrder ();
      RefreshAll ();

      //to Sibling 
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.LockLeave, TChild.Display, TypeInfo);
      DelegateCommand.PublishInternalMessage.Execute (message);
    }
    #endregion

    #region Dispacther
    void RequestModelDispatcher (TEntityAction action)
    {
      Model.RequestModel (action);

      //to Sibling 
      var message = new TFactorySiblingMessageInternal (TInternalMessageAction.Response, TChild.Display, TypeInfo);
      message.Support.Argument.Types.Select (action);
      
      DelegateCommand.PublishInternalMessage.Execute (message);
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
    void RefreshAll ()
    {
      if (FrameworkElementView.FindName ("DocumentControl") is Shared.Gadget.Document.TComponentDisplayControl documentControl) {
        documentControl.RefreshDesign ();
      }

      if (FrameworkElementView.FindName ("ImageControl") is Shared.Gadget.Image.TComponentControl imageControl) {
        imageControl.RefreshDesign ();
      }

      RefreshCollection ("FrameItemsViewSource");

      RaiseChanged ();
    }
    #endregion
  };
  //---------------------------//

}  // namespace