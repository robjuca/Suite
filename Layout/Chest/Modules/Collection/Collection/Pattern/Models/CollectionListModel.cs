/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Server.Models.Component;
using Shared.ViewModel;
//---------------------------//

namespace Layout.Collection.Pattern.Models
{
  public class TCollectionListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> ItemsCollection
    {
      get;
      private set;
    }

    public int SelectedIndex
    {
      get;
      set;
    }

    public string Title
    {
      get
      {
        return ($"[ {ItemsCollection.Count} ]");
      }
    }

    internal Guid CurrentId
    {
      get
      {
        return (Current.Id);
      }
    }

    internal TComponentModelItem Current
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
      Cleanup ();
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      /*
       DATA:
       action.CollectionAction.ModelCollection [id] {ModelAction {model}}
       */

      action.ThrowNull ();

      foreach (var model in action.CollectionAction.ModelCollection) {
        var modelBase = TComponentModel.Create (model.Value);
        var componentModelItem = TComponentModelItem.Create (modelBase);
        componentModelItem.Select (action.CategoryType.Category);

        ItemsCollection.Add (componentModelItem);
      }

      SelectedIndex = ItemsCollection.Count.Equals (0) ? -1 : 0;

      SortCollection ();
    }

    internal void ActiveChanged (TEntityAction action)
    {
      action.ThrowNull ();

      // remove old
      foreach (var item in ItemsCollection) {
        item.ClearActiveStatus ();
      }

      if (CurrentId.Equals (action.Id)) {
        Current.SelectActiveStatus (action.ModelAction.ComponentStatusModel.Active);
      }
    }

    internal void Cleanup ()
    {
      ItemsCollection = new ObservableCollection<TComponentModelItem> ();

      SelectedIndex = -1;
    }
    #endregion
    
    #region Support
    void SortCollection ()
    {
      var list = new ObservableCollection<TComponentModelItem> (ItemsCollection);

      ItemsCollection.Clear ();

      foreach (var item in list.OrderBy (p => p.Name)) {
        ItemsCollection.Add (item);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
