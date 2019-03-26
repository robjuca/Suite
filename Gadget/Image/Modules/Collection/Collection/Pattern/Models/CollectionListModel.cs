/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public class TCollectionListModel
  {
    #region Property
    public TStyleComponentModel StyleComponentModel
    {
      get;
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
        return ($"{CurrentStyleString}  [ {StyleComponentModel.ItemsCount} ]");
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
      StyleVerticalSelectorSelect = string.IsNullOrEmpty (StyleVerticalSelectorSelect) ? TContentStyle.MINI : StyleVerticalSelectorSelect;

      SelectStyle (m_SelectedStyleHorizontal, m_SelectedStyleVertical, action);
    }

    internal void SelectStyleHorizontal (TContentStyle.Style selectedStyleHorizontal)
    {
      StyleHorizontalSelectorModel.Select (selectedStyleHorizontal);
      StyleHorizontalSelectorSelect = selectedStyleHorizontal.ToString ();
      m_SelectedStyleHorizontal = selectedStyleHorizontal;

      Populate ();
    }

    internal void SelectStyleVertical (TContentStyle.Style selectedStyleVertical)
    {
      StyleVerticalSelectorModel.Select (selectedStyleVertical);
      StyleVerticalSelectorSelect = selectedStyleVertical.ToString ();
      m_SelectedStyleVertical = selectedStyleVertical;

      Populate ();
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
    TContentStyle.Style                                         m_SelectedStyleHorizontal;
    TContentStyle.Style                                         m_SelectedStyleVertical;
    //Guid                                                        m_CurrentId;
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
