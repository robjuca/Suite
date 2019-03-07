/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.ComponentModel.Composition;

using Caliburn.Micro;

using rr.Library.Types;
using rr.Library.Helper;
using rr.Library.Services;
using rr.Library.Infrastructure;
using rr.Library.Message;

using Shared.Types;
using Shared.Resources;
using Shared.Message;
//---------------------------//

namespace Shared.Services.Presentation
{
  [Export (typeof (IServicesPresentation))]
  public class TServicesPresentation : TPresentation, IHandleMessageModule, IServicesPresentation
  {
    [ImportingConstructor]
    #region Constructor
    public TServicesPresentation (IEventAggregator events)
      : base (events)
    {
      TypeName = GetType ().Name;

      DelegateCommand = new TPresentationCommand (this);

      m_EntityService = new TEntityService (this);
    }
    #endregion

    #region IHandle
    public void Handle (TMessageModule message)
    {
      // shell
      if (message.IsModule (TResource.TModule.Shell)) {
        // SettingsValidating
        if (message.IsAction (TMessageAction.SettingsValidating)) {
          TDispatcher.Invoke (SettingsValidatingDispatcher);
        }

        // Request
        if (message.IsAction (TMessageAction.Request)) {
          TDispatcher.BeginInvoke (RequestDispatcher, TServiceRequest.Create (message));
        }
      }

      // collection
      if (message.IsModule (TResource.TModule.Collection)) {
        // Request
        if (message.IsAction (TMessageAction.Request)) {
          TDispatcher.BeginInvoke (RequestDispatcher, TServiceRequest.Create (message));
        }
      }

      // factory
      if (message.IsModule (TResource.TModule.Factory)) {
        // Request
        if (message.IsAction (TMessageAction.Request)) {
          TDispatcher.BeginInvoke (RequestDispatcher, TServiceRequest.Create (message));
        }
      }
    }
    #endregion

    #region Presentation Command
    internal void NotifyDatabaseErrorHandler (TErrorMessage errorMessage)
    {
      //notify shell database error
      var message = new TServicesMessage (TMessageAction.SettingsValidated, TypeInfo);
      message.Support.Select (TActionStatus.Error);

      PublishInvoke (message);

      // show error message
      var msg = new TServicesMessage (TMessageAction.Error, TypeInfo);
      msg.Support.ErrorMessage.CopyFrom (errorMessage);

      PublishInvoke (msg);
    }
    #endregion

    #region Callback
    public void OnOperationComplete (TServiceArgs<Server.Models.Infrastructure.IEntityAction> args)
    {
      if (args.NotNull ()) {
        if (args.IsResultNullOrError.IsFalse ()) {
          var action = args.Result as Server.Models.Infrastructure.IEntityAction;

          // notify module
          var message = new TServicesMessage (action.Result, TMessageAction.Response, TypeInfo);
          (action.Param1 as TServiceRequest).Request (message);
          message.Support.Argument.Types.Select (action);

          PublishInvoke (message);
        }
      }

      PublishModalLeave ();
      PublishReportClear ();
    }
    #endregion

    #region Dispatcher
    void SettingsValidatingDispatcher ()
    {
      var filePath = System.Environment.CurrentDirectory;
      var fileName = TNames.SettingsIniFileName;

      var data = new TDatabaseConnection (filePath, fileName);

      // success
      if (data.Request ()) {
        if (data.IsValidate) {
          SelectConnectionString (data);

          // notify shell database success
          var message = new TServicesMessage (TMessageAction.SettingsValidated, TypeInfo);
          message.Support.Select (TActionStatus.Success);
          message.Support.Argument.Types.Select (data.Authentication);

          PublishInvoke (message);
        }

        else {
          var errorMessage = new TErrorMessage ("Settings ERROR", "Load Settings Dispatcher", "Database Connection String not validated!")
          {
            Severity = TSeverity.Hight
          };

          // notify shell database error
          NotifyDatabaseErrorHandler (errorMessage);
        }
      }

      else {
        var errorMessage = new TErrorMessage ("Settings ERROR", "Load Settings Dispatcher", (string) data.Result.ErrorContent)
        {
          Severity = TSeverity.Hight
        };

        // notify shell database error
        NotifyDatabaseErrorHandler (errorMessage);
      }
    }

    void RequestDispatcher (TServiceRequest serviceRequest)
    {
      // database
      if (serviceRequest.IsDatabase) {
        string reportMessage = $"{serviceRequest.EntityAction.Operation.CategoryType.Category} - {serviceRequest.EntityAction.Operation.Operation}";

        if (serviceRequest.EntityAction.Operation.HasExtension) {
          reportMessage += $" - {serviceRequest.EntityAction.Operation.Extension}";
        }

        PublishModalEnter ();
        PublishReportShow (reportMessage);

        serviceRequest.EntityAction.ConnectionString = DatabaseConnectionString;
        serviceRequest.EntityAction.Param1 = serviceRequest;

        m_EntityService.Operation (serviceRequest.Category, new TServiceAction<Server.Models.Infrastructure.IEntityAction> (serviceRequest.EntityAction, OnOperationComplete));
      }
    }
    #endregion

    #region Fields
    readonly TEntityService                           m_EntityService;
    #endregion

    #region Property
    string DatabaseConnectionString
    {
      get;
      set;
    }
    #endregion

    #region Support
    void SelectConnectionString (TDatabaseConnection data)
    {
      DatabaseConnectionString = data.CurrentDatabase.RequestConnectionString ();
    }

    void PublishReportShow (string reportMessage)
    {
      var msg = new TServicesMessage (TMessageAction.ReportShow, TypeInfo);
      msg.Support.Argument.Types.ReportData.Select (reportMessage);

      Publish (msg);
    }

    void PublishModalEnter () => Publish (new TServicesMessage (TMessageAction.ModalEnter, TypeInfo));

    void PublishModalLeave () => Publish (new TServicesMessage (TMessageAction.ModalLeave, TypeInfo));

    void PublishReportClear () => Publish (new TServicesMessage (TMessageAction.ReportClear, TypeInfo));
    #endregion
  };
  //---------------------------//

}  // namespace