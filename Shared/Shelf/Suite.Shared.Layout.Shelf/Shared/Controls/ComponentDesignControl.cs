/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;

using Shared.Types;
//---------------------------//

namespace Shared.Layout.Shelf
{
  public sealed class TComponentDesignControl : TComponentControlBase
  {
    #region Constructor
    public TComponentDesignControl ()
      : base (TControlMode.Design)
    {
      VerticalAlignment = VerticalAlignment.Center;

      //Child = m_ContentContainer;
    }
    #endregion

    #region Overrides
    

    //public override void RequestBorder (Border border)
    //{
    //  border.Margin = new Thickness (2);
    //  border.Padding = new Thickness (1);
    //  border.BorderThickness = new Thickness (.5);
    //  border.BorderBrush = Brushes.DarkGray;
    //}
    #endregion
  };
  //---------------------------//

}  // namespace