/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public ObservableCollection<TItemInfo> ItemsCollection
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

    public bool IsEmpty
    {
      get
      {
        return (ItemsCollection.Count.Equals (0));
      }
    }

    public TComponentModelItem Current
    {
      get
      {
        return (SelectedIndex.Equals (-1) ? null : ItemsCollection [SelectedIndex].Model);
      }
    }
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ItemsCollection = new ObservableCollection<TItemInfo> ();

      StyleSelectorModel = TStyleSelectorModel.CreateDefault;

      SelectedIndex = -1;

      m_NodeCollection = new Collection<ExtensionNode> ();
    }
    #endregion

    #region Members
    internal void Select (TEntityAction action)
    {
      /*
       - action.CollectionAction.ModelCollection {Id, Model}
       - action.CollectionAction.ExtensionNodeCollection
      */

      action.ThrowNull ();

      m_NodeCollection = new Collection<ExtensionNode> (action.CollectionAction.ExtensionNodeCollection);

      StyleSelectorModel.SelectItem (action);

      StyleSelectorSelect = string.IsNullOrEmpty (StyleSelectorSelect) ? TContentStyle.MINI : StyleSelectorSelect;

      SelectStyle (m_SelectedStyle);
    }

    internal void SelectStyle (TContentStyle.Style selectedStyle)
    {
      StyleSelectorModel.Select (selectedStyle);

      ItemsCollection.Clear ();

      var models = StyleSelectorModel.Request (selectedStyle).ItemsCollection;

      foreach (var componentModelItem in models) {
        IList<ExtensionNode> list = m_NodeCollection
          .Where (p => p.ParentId.Equals (componentModelItem.Id))
          .ToList ()
        ;

        componentModelItem.Select (list);

        ItemsCollection.Add (new TItemInfo (componentModelItem));
      }

      IsEnabledFilter = (ItemsCollection.Count > 0);
      SelectedIndex = ItemsCollection.Count.Equals (0) ?-1 : 0;

      m_SelectedStyle = selectedStyle;
    }

    internal void Cleanup ()
    {
      IsEnabledFilter = false;
      SelectedIndex = -1;
    }
    #endregion

    #region Fields
    TContentStyle.Style                                         m_SelectedStyle;
    Collection<ExtensionNode>                                   m_NodeCollection;
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
