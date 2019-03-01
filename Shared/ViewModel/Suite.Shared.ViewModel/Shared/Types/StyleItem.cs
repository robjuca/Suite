/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
//---------------------------//

namespace Shared.ViewModel
{
  public abstract class TStyleItem<I> 
  {
    #region Property
    public ObservableCollection<I> ItemsCollection
    {
      get;
      private set;
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public string Style
    {
      get
      {
        return (MyStyle??ItemStyle);
      }
    }
    #endregion

    #region Constructor
    protected TStyleItem (string style)
    {
      ItemStyle = style;

      SelectedIndex = -1;

      ItemsCollection = new ObservableCollection<I> ();
      m_Collection = new Collection<I> ();
      m_LockedItems = new Collection<Guid> ();
    }
    #endregion

    #region Virtual Members
    public virtual I GetCurrent ()
    {
      return ((I) new object ());
    }

    public virtual void SelectItem (Server.Models.Component.TEntityAction entityAction)
    {
    }
    #endregion

    #region Protected Property
    protected string MyStyle
    {
      get;
      set;
    }

    protected string ItemStyle
    {
      get;
      set;
    }

    protected bool HasItems
    {
      get
      {
        return (ItemsCollection.Count > 0);
      }
    }
    #endregion

    #region Fields
    readonly Collection<I>                            m_Collection;
    readonly Collection<Guid>                         m_LockedItems;
    #endregion
  };
  //---------------------------//

}
