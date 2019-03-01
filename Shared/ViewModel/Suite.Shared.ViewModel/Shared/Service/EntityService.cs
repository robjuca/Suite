/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Threading.Tasks;
//---------------------------//

namespace Shared.ViewModel
{
  //----- TErrorEventArgs
  public class TErrorEventArgs
  {
    #region Property
    public rr.Library.Types.TErrorMessage Error
    {
      get;
    }
    #endregion

    #region Constructor
    public TErrorEventArgs (rr.Library.Types.TErrorMessage error)
    {
      Error = rr.Library.Types.TErrorMessage.CreateDefault;
      Error.CopyFrom (error);
    }
    #endregion
  };
  //---------------------------//

  //----- TEntityService
  public class TEntityService : Server.Models.Infrastructure.IEntityOperation
  {
    #region Property
    public Server.Models.Infrastructure.TEntityService<Server.Models.Infrastructure.IEntityDataContext> Service
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TEntityService ()
    {
    }
    #endregion

    #region Interface Members
    public void Operation (rr.Library.Services.TServiceAction<Server.Models.Infrastructure.IEntityAction> serviceAction)
    {
      if (serviceAction.NotNull ()) {
        OperationAsync (serviceAction).ContinueWith (delegate
        {
          //?
        });
      }
    }
    #endregion

    #region Event
    public delegate void ShowErrorHandler (object sender, TErrorEventArgs e);

    public event ShowErrorHandler ShowError;
    #endregion

    #region Members
    public void SelectService (Server.Models.Infrastructure.TEntityService<Server.Models.Infrastructure.IEntityDataContext> service)
    {
      if (service.NotNull ()) {
        Service = service;
      }
    } 
    #endregion

    #region Await
    async Task OperationAsync (rr.Library.Services.TServiceAction<Server.Models.Infrastructure.IEntityAction> serviceAction)
    {
      if (Service.NotNull ()) {
        var param = serviceAction.Param as Server.Models.Infrastructure.IEntityAction;
        string messageError = $"[{param.Operation.CategoryType.Category} - {param.Operation.Operation}]";

        try {
          var task = Service.OperationAsync (param);

          if (task == await Task.WhenAny (task)) {
            if (task.Result.Result.IsValid == false) {
              var error = new rr.Library.Types.TErrorMessage ("Database ERROR Services", messageError, task.Result.Result.ErrorContent as string)
              {
                Severity = rr.Library.Types.TSeverity.Low
              };


              ErrorToShow (error);
            }

            serviceAction.ServiceArgs.Complete (task.Result, null);
          }
        }

        catch (Exception exception) {
          string msg = rr.Library.Helper.THelper.ExceptionStringFormat (serviceAction.ServiceArgs.CompletedCallbackName, exception);

          var error = new rr.Library.Types.TErrorMessage ("Database ERROR", messageError, msg)
          {
            Severity = rr.Library.Types.TSeverity.Low
          };

          ErrorToShow (error);

          serviceAction.ServiceArgs.Error = exception;
          serviceAction.ServiceArgs.Complete (param, null);
        }
      }
    }
    #endregion

    #region Static
    public static TEntityService CreateDefault => new TEntityService ();
    #endregion

    #region Support
    void ErrorToShow (rr.Library.Types.TErrorMessage error)
    {
      ShowError?.Invoke (this, new TErrorEventArgs (error));
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
