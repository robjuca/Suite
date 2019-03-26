/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public sealed class TStyleSelectorModel : TStyleSelectorModel<TStyleModelItem>
  {
    #region Constructor
    TStyleSelectorModel (TContentStyle.Mode styleMode)
      : base (
          TStyleModelItem.CreateMini (styleMode), 
          TStyleModelItem.CreateSmall (styleMode), 
          TStyleModelItem.CreateLarge (styleMode), 
          TStyleModelItem.CreateBig (styleMode), 
          TStyleModelItem.CreateNone (styleMode)
        )
    {
    }
    #endregion

    #region Overrides
    public override void SelectItem (TStyleModelItem styleItem, Server.Models.Component.TEntityAction action)
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