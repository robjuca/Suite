/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.ComponentModel.Composition.Hosting;

using rr.Library.Infrastructure;
//---------------------------//

namespace Module.Settings
{
  public class TBootstrapper : TBootstrapper<Shared.ViewModel.IShellViewModel>
  {
    #region Overrides
    protected override void ConfigureCatalog ()
    {
      // modules
      AddToCatalog (new AssemblyCatalog (typeof (Module.Settings.Factory.TModuleCatalog).Assembly));
      AddToCatalog (new AssemblyCatalog (typeof (Shared.Services.TModuleCatalog).Assembly));

      // create instance
      GetInstance (typeof (Shared.Services.Presentation.IServicesPresentation), null);
    }
    #endregion
  };
  //---------------------------//

}  // namespace