/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System.Collections.ObjectModel;

using Shared.Types;
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

    public TContentStyle.Mode StyleMode
    {
      get
      {
        return (StyleInfo.StyleMode);
      }
    }

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

      SelectedIndex = -1;

      ItemsCollection = new ObservableCollection<I> ();
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
    protected bool HasItems
    {
      get
      {
        return (ItemsCollection.Count > 0);
      }
    }
    #endregion
  };
  //---------------------------//

}
