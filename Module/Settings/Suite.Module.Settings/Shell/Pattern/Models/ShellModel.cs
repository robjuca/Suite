/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.ViewModel;
using Shared.Types;
//---------------------------//

namespace Module.Settings.Shell.Pattern.Models
{
  public class TShellModel : TShellModelReference
  {
    #region Property
    public TSnackbarMessage SnackbarContent
    {
      get;
    }
    #endregion

    #region Constructor
    public TShellModel ()
    {
      SnackbarContent = TSnackbarMessage.CreateDefault;
    }
    #endregion
  };
  //---------------------------//

}  // namespace