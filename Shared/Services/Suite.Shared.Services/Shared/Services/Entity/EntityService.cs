/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Services;
using rr.Library.Types;
//---------------------------//

namespace Shared.Services
{
  public class TEntityService : rr.Library.Infrastructure.TViewModelPresentation
  {
    #region Constructor
    public TEntityService (Presentation.IServicesPresentation presentation)
    {
      presentation.RequestPresentationCommand (this);

      m_EntityService = Shared.ViewModel.TEntityService.CreateDefault;
      m_EntityService.ShowError += ServiceShowError;
      m_EntityService.SelectService (new Server.Services.Component.TEntityServiceAsync ());
    }
    #endregion

    #region Members
    internal void Operation (Server.Models.Infrastructure.TCategory category, TServiceAction<Server.Models.Infrastructure.IEntityAction> serviceAction)
    {
      if (category.Equals (Server.Models.Infrastructure.TCategory.None).IsFalse ()) {
        m_EntityService.Operation (serviceAction);
      }
    } 
    #endregion

    #region Property
    Presentation.IDelegateCommand DelegateCommand
    {
      get
      {
        return (PresentationCommand as Presentation.IDelegateCommand);
      }
    }
    #endregion

    #region Event
    void ServiceShowError (object sender, Shared.ViewModel.TErrorEventArgs e)
    {
      ShowError (e.Error);
    }
    #endregion

    #region Fields
    Shared.ViewModel.TEntityService                             m_EntityService;
    #endregion

    #region Support
    void ShowError (TErrorMessage error)
    {
      DelegateCommand.NotifyDatabaseError.Execute (error);
    }
    #endregion
  };
  //---------------------------//

}  // namespace