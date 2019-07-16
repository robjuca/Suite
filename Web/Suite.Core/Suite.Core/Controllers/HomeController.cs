/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
//---------------------------//

namespace Suite.Controllers
{
  public class HomeController : Controller
  {
    #region Constructor
    public HomeController (IHostingEnvironment environment, Server.Context.Component.TModelContext modelDbContext)
    {
      m_HostingEnvironment = environment;
      m_ModelDbContext = modelDbContext;
    }
    #endregion

    #region Members
    public IActionResult Index ()
    {
      var model = Suite.Core.ViewModel.TComponentModelItem.CreateDefault;
      model.SelectWebRootPath (m_HostingEnvironment.WebRootPath);

      var action = Server.Models.Component.TEntityAction.CreateDefault;
      Server.Context.Component.TEntityDataContext.SelectActive (m_ModelDbContext, action);

      if (action.Result.IsValid) {
        model = Suite.Core.ViewModel.TComponentModelItem.Create (action);
        model.Select (Server.Models.Infrastructure.TCategoryType.FromValue ((int) action.Param1));
        model.RequestChild (action);
      }

      else {
        //return View (new Suite.Models.ErrorViewModel { Error = (string) action.Result.ErrorContent });
        return Error ();
      }

      return View (model);
    }

    public IActionResult Privacy ()
    {
      return View ();
    }

    [ResponseCache (Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error ()
    {
      return View (new Suite.Models.ErrorViewModel { Error = "error" });
      //return View ();
    }
    #endregion

    #region Fields
    readonly Server.Context.Component.TModelContext m_ModelDbContext;
    readonly IHostingEnvironment m_HostingEnvironment;
    #endregion
  };
  //---------------------------//

}  // namespace