/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Windows;
using System.Windows.Controls;
//---------------------------//

namespace Shared.Resources
{
  [TemplatePart (Name = PART_MODULE_NAME, Type = typeof (TextBlock))]
  public sealed class TFooter : Control
  {
    #region Constructor
    public TFooter ()
    {
      DefaultStyleKey = typeof (TFooter);
    }
    #endregion

    #region Overrides
    public override void OnApplyTemplate ()
    {
      base.OnApplyTemplate ();

      if (GetTemplateChild (PART_MODULE_NAME) is TextBlock text) {
        text.Text = Name;
      }
    }
    #endregion

    #region Constants
    const string                            PART_MODULE_NAME = "PART_ModuleName";
    #endregion
  };
  //---------------------------//

}  // namespace