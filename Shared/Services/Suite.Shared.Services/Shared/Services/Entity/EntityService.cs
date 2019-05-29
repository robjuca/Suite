/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

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

      //m_Services = new Dictionary<Server.Models.Infrastructure.TCategory, Shared.ViewModel.TEntityService> ();
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

    #region Overrides
    //protected override void Initialize ()
    //{
    //  if (m_Services.Count.Equals (0)) {
    //    foreach (int item in Enum.GetValues (typeof (Server.Models.Infrastructure.TCategory))) {
    //      var category = Server.Models.Infrastructure.TCategoryType.FromValue (item);

    //      if (category.Equals (Server.Models.Infrastructure.TCategory.None)) {
    //        continue;
    //      }

    //      else {
    //        var service = Shared.ViewModel.TEntityService.CreateDefault;
    //        service.ShowError += ServiceShowError;
    //        service.SelectService (new Server.Services.Component.TEntityServiceAsync ());
    //        m_Services.Add (category, service);
    //      }
    //    }
    //  }
    //}
    #endregion

    #region Fields
    //readonly Dictionary<Server.Models.Infrastructure.TCategory, Shared.ViewModel.TEntityService>                  m_Services;
    Shared.ViewModel.TEntityService                       m_EntityService;
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