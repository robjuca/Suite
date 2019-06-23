/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

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
    #endregion

    #region Constructor
    public TCollectionListModel ()
    {
      ItemsCollection = new ObservableCollection<TComponentModelItem> ();
      SelectedIndex = -1;
    }
    #endregion

    #region Members
    internal void Select (Server.Models.Component.TEntityAction action)
    {
      /*
       DATA:
       action.CollectionAction.ModelCollection [id] {ModelAction {model}}
       */

      if (action.NotNull ()) {
        foreach (var model in action.CollectionAction.ModelCollection) {
          var modelItem = Server.Models.Component.TComponentModel.Create (model.Value);
          var componentModelItem = TComponentModelItem.Create (modelItem);
          componentModelItem.Select (Server.Models.Infrastructure.TCategory.Shelf);

          ItemsCollection.Add (componentModelItem);
        }
      }

      SelectedIndex = ItemsCollection.Count.Equals (0) ? -1 : 0;
    }

    internal void Cleanup ()
    {
      ItemsCollection.Clear ();

      SelectedIndex = -1;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
