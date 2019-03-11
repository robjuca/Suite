/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public sealed class TStyleSelectorModel : TStyleSelectorModel<TStyleItem>
  {
    #region Constructor
    TStyleSelectorModel (TContentStyle.Mode styleMode)
      : base (
          TStyleItem.CreateMini (styleMode), 
          TStyleItem.CreateSmall (styleMode), 
          TStyleItem.CreateLarge (styleMode), 
          TStyleItem.CreateBig (styleMode), 
          TStyleItem.CreateNone (styleMode)
        )
    {
    }
    #endregion

    #region Overrides
    public override void SelectItem (TStyleItem styleItem, Server.Models.Component.TEntityAction action)
    {
      styleItem.SelectItem (action);
    }
    #endregion

    #region Property
    public static TStyleSelectorModel Create (TContentStyle.Mode styleMode) => new TStyleSelectorModel (styleMode);
    #endregion
  };
  //---------------------------//

}  // namespace