/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
//---------------------------//

namespace Suite.Launcher
{
  public partial class TApp : Application
  {
    #region Overrides
    protected override void OnStartup (StartupEventArgs e)
    {
      rr.Library.Types.TSingleInstance.Make ();

      base.OnStartup (e);
    }
    #endregion
  };
  //---------------------------//

}  // namespace