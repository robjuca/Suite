/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;

using Server.Models.Component;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Layout.Collection.Pattern.Models
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

    public int SlideIndex
    {
      get;
      set;
    }

    public bool IsEmpty
    {
      get
      {
        return (StyleComponentModel.IsEmpty);
      }
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

      m_NodeCollection = new Collection<ExtensionNode> ();
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

      m_NodeCollection = new Collection<ExtensionNode> (action.CollectionAction.ExtensionNodeCollection);
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
      IsEnabledFilter = false;
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
    Collection<ExtensionNode>                                   m_NodeCollection;
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

  //----- TItemInfo
  public class TItemInfo
  {
    #region Property
    public int Category
    {
      get;
    }

    public Server.Models.Infrastructure.TCategory ChildCategory
    {
      get;
    }

    public TComponentModelItem Model
    {
      get;
      set;
    }

    public Visibility DocumentVisibility
    {
      get;
      set;
    }

    public Visibility ImageVisibility
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TItemInfo (TComponentModelItem model)
      : this ()
    {
      Model.CopyFrom (model);

      if (Model.NodeModelCollection.Count > 0) {
        Category = Model.NodeModelCollection [0].ChildCategory;
        ChildCategory = Server.Models.Infrastructure.TCategoryType.FromValue (Category);
      }

      DocumentVisibility = ChildCategory.Equals (Server.Models.Infrastructure.TCategory.Document) ? Visibility.Visible : Visibility.Collapsed;
      ImageVisibility = ChildCategory.Equals (Server.Models.Infrastructure.TCategory.Image) ? Visibility.Visible : Visibility.Collapsed;
    }

    TItemInfo ()
    {
      Model = TComponentModelItem.CreateDefault;

      DocumentVisibility = Visibility.Collapsed;
      ImageVisibility = Visibility.Collapsed;

      ChildCategory = Server.Models.Infrastructure.TCategory.None;
      Category = Server.Models.Infrastructure.TCategoryType.ToValue (ChildCategory);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
