/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
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
        return (MyStyleString??ItemStyleString);
      }
    }
    #endregion

    #region Constructor
    protected TStyleItem (string style)
    {
      ItemStyleString = style;

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
    protected string MyStyleString
    {
      get;
      set;
    }

    protected string ItemStyleString
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
  };
  //---------------------------//

}
