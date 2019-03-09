/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Shared.Types;
//---------------------------//

namespace Shared.Layout.Shelf
{
  public sealed class TComponentDisplayControl : TComponentControlBase
  {
    #region Constructor
    public TComponentDisplayControl ()
      : base (TControlMode.Display)
    {
    }

    public TComponentDisplayControl (TComponentControlModel model)
      : base (TControlMode.Display, model)
    {
    }
    #endregion

    #region Overrides
    public override void CreateContentContainer ()
    {
      m_ContentContainer = new Grid ()
      {
        Background = new SolidColorBrush (Color.FromRgb (252, 252, 252))
      };

      //m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (306, GridUnitType.Pixel) }); // col 1
      //m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (306, GridUnitType.Pixel) }); // col 2
      //m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (306, GridUnitType.Pixel) }); // col 3
      //m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (306, GridUnitType.Pixel) }); // col 4

      m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }); // col 1
      m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }); // col 2
      m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }); // col 3
      m_ContentContainer.ColumnDefinitions.Add (new ColumnDefinition () { Width = new GridLength (1, GridUnitType.Auto) }); // col 4

      //m_ContentContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (122, GridUnitType.Pixel) }); // row 1
      //m_ContentContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (122, GridUnitType.Pixel) }); // row 2
      //m_ContentContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (122, GridUnitType.Pixel) }); // row 3

      m_ContentContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 1
      m_ContentContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 2
      m_ContentContainer.RowDefinitions.Add (new RowDefinition () { Height = new GridLength (1, GridUnitType.Auto) }); // row 3
    } 
    #endregion
  };
  //---------------------------//

}  // namespace