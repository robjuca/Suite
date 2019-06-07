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

using Gadget.Factory.Presentation;
using Gadget.Factory.Pattern.Models;
//---------------------------//

namespace Gadget.Factory.Pattern.ViewModels
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
            Model.PropertySelect (message.Support.Argument.Args.PropertyName, TEntityAction.Request (message.Support.Argument.Types.EntityAction));

            RaiseChanged ();

            //to Sibling (send Report)
            var messageInternal = new TFactorySiblingMessageInternal (TInternalMessageAction.Report, TChild.Display, TypeInfo);
            Model.RequestReport (messageInternal.Support.Argument.Types.ReportData);

            DelegateCommand.PublishInternalMessage.Execute (messageInternal);
          }

          // Select
          if (message.IsAction (TInternalMessageAction.Select)) {
            TDispatcher.BeginInvoke (SelectDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Request
          if (message.IsAction (TInternalMessageAction.Request)) {
            TDispatcher.BeginInvoke (RequestDispatcher, TEntityAction.Request (message.Support.Argument.Types.EntityAction));
          }

          // Cleanup
          if (message.IsAction (TInternalMessageAction.Cleanup)) {
            Model.Cleanup ();
            RaiseChanged ();
          }
        }
      }
    }
    #endregion

    #region Dispatcher
    void SelectDispatcher (TEntityAction action)
    {
      //Model.Select (action);
      RaiseChanged ();
    }

    void RequestDispatcher (TEntityAction action)
    {
      Model.RequestModel (action);

      //to Sibling (send Response)
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
  };
  //---------------------------//

}  // namespace