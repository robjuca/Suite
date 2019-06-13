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
    public TStyleComponentModel StyleComponentModel
    {
      get;
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
        return ($"{CurrentStyleString}  [ {StyleComponentModel. ItemsCount} ]");
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
        return (Current.Id);
      }
    }

    public TComponentModelItem Current
    {
      get
      {
        return (StyleComponentModel.RequestItem (SelectedIndex));
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      StyleComponentModel = TStyleComponentModel.CreateDefault;

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
      // DATA IN:
      // action.CollectionAction.ModelCollection

      action.ThrowNull ();

      StyleHorizontalSelectorSelect = string.IsNullOrEmpty (StyleHorizontalSelectorSelect) ? TContentStyle.MINI : StyleHorizontalSelectorSelect;
      StyleHorizontalSelectorSelect = StyleHorizontalSelectorSelect.Equals (TContentStyle.NONE) ? TContentStyle.MINI : StyleHorizontalSelectorSelect;
      m_SelectedStyleHorizontal = TContentStyle.TryToParse (StyleHorizontalSelectorSelect);

      StyleVerticalSelectorSelect = string.IsNullOrEmpty (StyleVerticalSelectorSelect) ? TContentStyle.MINI : StyleVerticalSelectorSelect;
      StyleVerticalSelectorSelect = StyleVerticalSelectorSelect.Equals (TContentStyle.NONE) ? TContentStyle.MINI : StyleVerticalSelectorSelect;
      m_SelectedStyleVertical = TContentStyle.TryToParse (StyleVerticalSelectorSelect);

      SelectStyle (m_SelectedStyleHorizontal, m_SelectedStyleVertical, action);
    }

    internal void Update (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      Current.Select (action);
    }

    internal void PreserveCurrent ()
    {
      m_CurrentId = Id;
    }

    internal void DiscardCurrent ()
    {
      m_CurrentId = Guid.Empty;
    }

    internal void TryToSelect ()
    {
      if (m_CurrentId.NotEmpty ()) {
        var item = StyleComponentModel.RequestComponentModel (m_CurrentId);

        if (item.Id.NotEmpty ()) {
          var styleHorizontal = TContentStyle.TryToParse (item.StyleHorizontal);
          var styleVertical = TContentStyle.TryToParse (item.StyleVertical);

          SelectStyleHorizontal (styleHorizontal);
          SelectStyleVertical (styleVertical);

          SelectedIndex = StyleComponentModel.RequestIndex (m_CurrentId);
        }
      }

      DiscardCurrent ();
    }

    internal void SelectStyleHorizontal (TContentStyle.Style selectedStyleHorizontal)
    {
      if (StyleHorizontalSelectorModel.Select (selectedStyleHorizontal)) {
        StyleHorizontalSelectorSelect = selectedStyleHorizontal.ToString ();
        m_SelectedStyleHorizontal = selectedStyleHorizontal;

        Populate ();
      }
    }

    internal void SelectStyleVertical (TContentStyle.Style selectedStyleVertical)
    {
      if (StyleVerticalSelectorModel.Select (selectedStyleVertical)) {
        StyleVerticalSelectorSelect = selectedStyleVertical.ToString ();
        m_SelectedStyleVertical = selectedStyleVertical;

        Populate ();
      }
    }

    internal void SelectStyle (TContentStyle.Style selectedStyleHorizontal, TContentStyle.Style selectedStyleVertical, Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      StyleComponentModel.Select (action);

      SelectStyleHorizontal (selectedStyleHorizontal);
      SelectStyleVertical (selectedStyleVertical);
    }

    internal void Cleanup ()
    {
      SelectedIndex = -1;

      SelectStyleHorizontal (TContentStyle.Style.None);
      SelectStyleVertical (TContentStyle.Style.None);
    }
    #endregion

    #region Property
    string CurrentStyleString
    {
      get
      {
        return ($"{StyleHorizontalSelectorModel.Current.StyleInfo.StyleFullString}, {StyleVerticalSelectorModel.Current.StyleInfo.StyleFullString}");
      }
    }
    #endregion

    #region Fields
    TContentStyle.Style                                                   m_SelectedStyleHorizontal;
    TContentStyle.Style                                                   m_SelectedStyleVertical;
    Guid                                                                  m_CurrentId;
    #endregion

    #region Support
    void Populate ()
    {
      StyleComponentModel.Select (m_SelectedStyleHorizontal, m_SelectedStyleVertical);

      IsEnabledFilter = StyleComponentModel.HasItems;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace
