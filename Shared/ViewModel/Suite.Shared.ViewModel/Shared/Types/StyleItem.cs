/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public abstract class TStyleItem
  {
    #region Property
    public TStyleInfo StyleInfo
    {
      get;
    }
    #endregion

    #region Constructor
    protected TStyleItem (TContentStyle.Mode styleMode, string style)
      : this ()
    {
      StyleInfo = TStyleInfo.Create (styleMode);
      StyleInfo.Select (style);
    }

    TStyleItem ()
    {
      StyleInfo = TStyleInfo.CreateDefault;
    }
    #endregion

    #region Virtual Members
    public virtual void SelectItem (Server.Models.Component.TEntityAction entityAction)
    {
    }
    #endregion
  };
  //---------------------------//

}
