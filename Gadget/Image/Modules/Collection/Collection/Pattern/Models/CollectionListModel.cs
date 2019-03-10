/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Component;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> ItemsCollection
    {
      get;
      private set;
    }

    public TStyleSelectorModel StyleSelectorModel
    {
      get;
      private set;
    }


    public string Title
    {
      get
      {
        return ($"{StyleSelectorModel.Current.Style} [ {StyleSelectorModel.Current.ItemsCollection.Count} ]");
      }
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public string StyleSelectorSelect
    {
      get;
      set;
    }

    public bool IsEnabledFilter
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ItemsCollection = new ObservableCollection<TComponentModelItem> ();

      StyleSelectorModel = TStyleSelectorModel.CreateDefault;

      SelectedIndex = -1;
    }
    #endregion

    #region Members
    internal void Select (TEntityAction action)
    {
      /*
       action.CollectionAction.ModelCollection {Id, Model}
       */

      action.ThrowNull ();

      StyleSelectorModel.SelectItem (action);

      StyleSelectorSelect = string.IsNullOrEmpty (StyleSelectorSelect) ? TContentStyle.MINI : StyleSelectorSelect;

      SelectStyle (m_SelectedStyle);
    }

    internal TComponentModelItem RequestCurrent (TContentStyle.Style style)
    {
      return (StyleSelectorModel.Request (style).GetCurrent ());
    }

    internal void SelectStyle (TContentStyle.Style selectedStyle)
    {
      StyleSelectorModel.Select (selectedStyle);

      ItemsCollection = new ObservableCollection<TComponentModelItem> (StyleSelectorModel.Request (selectedStyle).ItemsCollection);
      IsEnabledFilter = (ItemsCollection.Count > 0);

      m_SelectedStyle = selectedStyle;
    }

    internal void Cleanup ()
    {
      SelectedIndex = -1;
    }
    #endregion

    #region Fields
    TContentStyle.Style                     m_SelectedStyle;
    #endregion
  };
  //---------------------------//

}  // namespace
