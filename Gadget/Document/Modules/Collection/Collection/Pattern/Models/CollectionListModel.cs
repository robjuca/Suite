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

    public TStyleSelectorModel StyleHorizontalSelectorModel
    {
      get;
      private set;
    }

    public TStyleSelectorModel StyleVerticalSelectorModel
    {
      get;
      private set;
    }

    public string Title
    {
      get
      {
        return ($"{CurrentStyleString} [ {StyleHorizontalSelectorModel.Current.ItemsCollection.Count} ]");
      }
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public string StyleHorizontalSelectorSelect
    {
      get;
      set;
    }

    public string StyleVerticalSelectorSelect
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

      StyleHorizontalSelectorModel = TStyleSelectorModel.Create (TContentStyle.Mode.Horizontal);
      StyleHorizontalSelectorSelect = string.Empty;

      StyleVerticalSelectorModel = TStyleSelectorModel.Create (TContentStyle.Mode.Vertical);
      StyleVerticalSelectorSelect = string.Empty;

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

      m_SelectedStyleHorizontal = TContentStyle.Style.mini;
      m_SelectedStyleVertical = TContentStyle.Style.mini;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      StyleHorizontalSelectorModel.SelectItem (action);
      StyleHorizontalSelectorSelect = string.IsNullOrEmpty (StyleHorizontalSelectorSelect) ? TContentStyle.MINI : StyleHorizontalSelectorSelect;

      StyleVerticalSelectorModel.SelectItem (action);
      StyleVerticalSelectorSelect = string.IsNullOrEmpty (StyleVerticalSelectorSelect) ? TContentStyle.MINI : StyleVerticalSelectorSelect;

      SelectStyle (m_SelectedStyleHorizontal, m_SelectedStyleVertical);
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
          var styleHorizontalItem = StyleHorizontalSelectorModel.Request (style);
          var styleVerticalItem = StyleVerticalSelectorModel.Request (style);

          // horizontal
          for (int index = 0; index < styleHorizontalItem.ItemsCollection.Count; index++) {
            var itemModel = styleHorizontalItem.ItemsCollection [index];

            if (itemModel.Id.Equals (m_Current)) {
              //TODO: Review
              //StyleSelectorSelect = itemModel.Style;
              //SelectedIndex = index;

              break;
            }
          }

          // vertical
          for (int index = 0; index < styleVerticalItem.ItemsCollection.Count; index++) {
            var itemModel = styleVerticalItem.ItemsCollection [index];

            if (itemModel.Id.Equals (m_Current)) {
              //TODO: Review
              //StyleSelectorSelect = itemModel.Style;
              //SelectedIndex = index;

              break;
            }
          }
        }
      }

      DiscardCurrent ();
    }

    internal void SelectStyleHorizontal (TContentStyle.Style selectedStyleHorizontal)
    {
      StyleHorizontalSelectorModel.Select (selectedStyleHorizontal);
      StyleHorizontalSelectorSelect = selectedStyleHorizontal.ToString ();
      m_SelectedStyleHorizontal = selectedStyleHorizontal;
    }

    internal void SelectStyleVertical (TContentStyle.Style selectedStyleVertical)
    {
      StyleVerticalSelectorModel.Select (selectedStyleVertical);
      StyleVerticalSelectorSelect = selectedStyleVertical.ToString ();
      m_SelectedStyleVertical = selectedStyleVertical;
    }

    internal void SelectStyle (TContentStyle.Style selectedStyleHorizontal, TContentStyle.Style selectedStyleVertical)
    {
      SelectStyleHorizontal (selectedStyleHorizontal);
      SelectStyleVertical (selectedStyleVertical);

      //TODO: Review
      var horizontalList = StyleHorizontalSelectorModel.Request (selectedStyleHorizontal).ItemsCollection;
      var verticalList = StyleVerticalSelectorModel.Request (selectedStyleVertical).ItemsCollection;

      //ItemsCollection = new ObservableCollection<TComponentModelItem> (StyleHorizontalSelectorModel.Request (selectedStyleHorizontal).ItemsCollection);

      IsEnabledFilter = (ItemsCollection.Count > 0);
    }

    internal void Cleanup ()
    {
      SelectedIndex = -1;
    }
    #endregion

    #region Property
    string CurrentStyleString
    {
      get
      {
        return ($"{StyleHorizontalSelectorModel.Current.StyleInfo.StyleString}, {StyleVerticalSelectorModel.Current.StyleInfo.StyleString}");
      }
    } 
    #endregion

    #region Fields
    TContentStyle.Style                                         m_SelectedStyleHorizontal;
    TContentStyle.Style                                         m_SelectedStyleVertical;
    Guid                                                        m_Current;
    #endregion
  };
  //---------------------------//

}  // namespace
