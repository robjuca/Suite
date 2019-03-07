/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

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

    public TComponentModelItem ComponentModelItem
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TShellModel ()
    {
      SnackbarContent = TSnackbarMessage.CreateDefault;
      ComponentModelItem = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelItem = TComponentModelItem.Create (action);
    } 
    #endregion
  };
  //---------------------------//

}  // namespace