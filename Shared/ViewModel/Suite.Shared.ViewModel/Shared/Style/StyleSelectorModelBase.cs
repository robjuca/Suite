/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;

using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  public abstract class TStyleSelectorModel<S>
  {
    #region Property
    public S StyleMini
    {
      get;
      private set;
    }

    public S StyleSmall
    {
      get;
      private set;
    }

    public S StyleLarge
    {
      get;
      private set;
    }

    public S StyleBig
    {
      get;
      private set;
    }

    public S StyleNone
    {
      get;
      private set;
    }

    public S Current
    {
      get
      {
        return (m_Styles [m_SelectedStyle]);
      }
    }
    #endregion

    #region Constructor
    public TStyleSelectorModel (S styleMini, S styleSmall, S styleLarge, S styleBig, S styleNone)
    {
      StyleMini = styleMini;
      StyleSmall = styleSmall;
      StyleLarge = styleLarge;
      StyleBig = styleBig;
      StyleNone = styleNone;

      m_Styles = new Dictionary<TContentStyle.Style, S>
      {
        { TContentStyle.Style.mini, StyleMini },
        { TContentStyle.Style.small, StyleSmall },
        { TContentStyle.Style.large, StyleLarge },
        { TContentStyle.Style.big, StyleBig },
        { TContentStyle.Style.None, StyleNone }
      };

      m_SelectedStyle = TContentStyle.Style.None;
    }
    #endregion
    
    #region Virtual Members
    public virtual void SelectItem (S styleItem, Server.Models.Component.TEntityAction action)
    {
    }

    public virtual void SelectContent (S styleItem, Server.Models.Component.TEntityAction action)
    {
    }
    #endregion

    #region Members
    public S Request (TContentStyle.Style style)
    {
      return (m_Styles [style]);
    }

    public bool Select (TContentStyle.Style selectedStyle)
    {
      bool res = false;

      if (m_SelectedStyle.Equals (selectedStyle).IsFalse ()) {
        m_SelectedStyle = selectedStyle;
        res = true;
      }

      return (res);
    }

    public void SelectItem (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        foreach (var item in m_Styles) {
          SelectItem (item.Value, action);
        }
      }
    }

    public void SelectContent (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        foreach (var item in m_Styles) {
          SelectContent (item.Value, action);
        }
      }
    }
    #endregion

    #region Fields
    readonly Dictionary<TContentStyle.Style, S>                           m_Styles;
    TContentStyle.Style                                                   m_SelectedStyle; 
    #endregion
  };
  //---------------------------//

}  // namespace