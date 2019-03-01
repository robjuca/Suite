/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.ViewModel
{
  public sealed class TStyleSelectorModel : TStyleSelectorModel<TStyleItem>
  {
    #region Constructor
    TStyleSelectorModel ()
      : base (TStyleItem.CreateMini, TStyleItem.CreateSmall, TStyleItem.CreateLarge, TStyleItem.CreateBig, TStyleItem.CreateNone)
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
    public static TStyleSelectorModel CreateDefault => new TStyleSelectorModel ();
    #endregion
  };
  //---------------------------//

}  // namespace