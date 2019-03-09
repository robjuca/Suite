/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
//---------------------------//

namespace Layout.Drawer
{
  public partial class TApp : Application
  {
    #region Overrides
    protected override void OnStartup (StartupEventArgs e)
    {
      if (e.Args.Length > 0) {
        var key = e.Args [0];

        if (key == "Layout.Drawer") {
          rr.Library.Types.TSingleInstance.Make ();

          base.OnStartup (e);
        }

        else {
          Shutdown ();
        }
      }

      else {
        Shutdown ();
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace