/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> ItemsCollection
    {
      get;
      private set;
    }

    public Collection<string> FilterEnabledItemsSource
    {
      get;
      private set;
    }

    public Collection<string> FilterPictureItemsSource
    {
      get;
      private set;
    }

    public TFilterInfo FilterInfo
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

    public int SlideIndex
    {
      get;
      set;
    }

    public bool IsEnabledFilter
    {
      get;
      set;
    }

    public Guid Id
    {
      get
      {
        return (SelectedIndex.Equals (-1) ? Guid.Empty : ItemsCollection [SelectedIndex].Id);
      }
    }

    public TComponentModelItem Current
    {
      get
      {
        return (SelectedIndex.Equals (-1) ? TComponentModelItem.CreateDefault : ItemsCollection [SelectedIndex]);
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ItemsCollection = new ObservableCollection<TComponentModelItem> ();

      StyleSelectorModel = TStyleSelectorModel.CreateDefault;
      StyleSelectorSelect = string.Empty;

      SelectedIndex = -1;
      SlideIndex = 0;

      FilterEnabledItemsSource = new Collection<string> ();

      foreach (var item in Enum.GetNames (typeof (TFilterEnabled))) {
        FilterEnabledItemsSource.Add (item);
      }

      FilterPictureItemsSource = new Collection<string> ();

      foreach (var item in Enum.GetNames (typeof (TFilterPicture))) {
        FilterPictureItemsSource.Add (item);
      }

      FilterInfo = TFilterInfo.CreateDefault;

      m_SelectedStyle = TContentStyle.Style.mini;
    }
    #endregion

    #region Members
    //internal void Preserve (Server.Models.Component.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  m_PreserveAction = action;
    //}

    internal void Select (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      StyleSelectorModel.SelectItem (action);

      StyleSelectorSelect = string.IsNullOrEmpty (StyleSelectorSelect) ? TContentStyle.MINI : StyleSelectorSelect;

      SelectStyle (m_SelectedStyle);
    }

    internal void Update (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      Current.Select (action);
    }

    internal void PreserveCurrent ()
    {
      m_Current = Id;
    }

    internal void DiscardCurrent ()
    {
      m_Current = Guid.Empty;
    }

    internal void TryToSelect ()
    {
      if (m_Current.NotEmpty ()) {
        foreach (TContentStyle.Style style in TContentStyle.GetValues) {
          var styleItem = StyleSelectorModel.Request (style);
          
          for (int index = 0; index < styleItem.ItemsCollection.Count; index++) {
            var itemModel = styleItem.ItemsCollection [index];

            if (itemModel.Id.Equals (m_Current)) {
              //TODO: Review
              //StyleSelectorSelect = itemModel.Style;
              SelectedIndex = index;

              break;
            }
          }
        }
      }

      DiscardCurrent ();
    }

    //internal TComponentModelItem RequestCurrent (TContentStyle.Style style)
    //{
    //  return (StyleSelectorModel.Request (style).GetCurrent ());
    //}

    internal void SelectStyle (TContentStyle.Style selectedStyle)
    {
      StyleSelectorModel.Select (selectedStyle);
      StyleSelectorSelect = selectedStyle.ToString ();

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
    //Server.Models.Component.TEntityAction                       m_PreserveAction;
    TContentStyle.Style                                         m_SelectedStyle;
    Guid                                                        m_Current;
    #endregion
  };
  //---------------------------//

}  // namespace
