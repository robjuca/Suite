/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  
  public sealed class TStyleComponentModel
  {
    #region Property
    public Collection<TComponentModelItem> ComponentModelCollection
    {
      get;
    }

    public ObservableCollection<TComponentModelItem> ItemsCollection
    {
      get;
      private set;
    }

    public int ItemsCount
    {
      get
      {
        return (ItemsCollection.Count);
      }
    }

    public bool IsEmpty
    {
      get
      {
        return (ItemsCount.Equals (0));
      }
    }

    public bool HasItems
    {
      get
      {
        return (ItemsCount > 0);
      }
    }
    #endregion

    #region Constructor
    TStyleComponentModel ()
    {
      ComponentModelCollection = new Collection<TComponentModelItem> ();
      ItemsCollection = new ObservableCollection<TComponentModelItem> ();
    }
    #endregion

    #region Members
    public void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection
      // action.CollectionAction.ExtensionNodeCollection

      if (action.NotNull ()) {
        ComponentModelCollection.Clear ();
        ItemsCollection.Clear ();

        foreach (var modelAction in action.CollectionAction.ModelCollection) {
          var componentModel = Server.Models.Component.TComponentModel.Create (modelAction.Value);

          var componentModelItem = TComponentModelItem.Create (componentModel);
          componentModelItem.Select (action.CategoryType.Category); 

          ComponentModelCollection.Add (componentModelItem);
        }

        foreach (var item in ComponentModelCollection) {
          item.PopulateNode (action);
        }
      }
    }

    public void Select (TContentStyle.Style selectedStyleHorizontal, TContentStyle.Style selectedStyleVertical)
    {
      if (ComponentModelCollection.Count > 0) {
        var list = ComponentModelCollection
          .Where (p => p.StyleHorizontal.Equals (selectedStyleHorizontal.ToString ()))
          .Where (p => p.StyleVertical.Equals (selectedStyleVertical.ToString ()))
          .ToList ()
        ;

        ItemsCollection = new ObservableCollection<TComponentModelItem> (list);
      }
    }

    public TComponentModelItem RequestItem (int index)
    {
      if ((index > -1) && (index < ItemsCount)) {
        return (ItemsCollection [index]);
      }

      return (TComponentModelItem.CreateDefault);
    }

    public int RequestIndex (Guid id)
    {
      var index = -1;

      for (int i = 0; i < ItemsCollection.Count; i++) {
        var item = ItemsCollection [i];

        if (item.Id.Equals (id)) {
          index = i;
          break;
        }
      }

      return (index);
    }

    public TComponentModelItem RequestComponentModel (Guid id)
    {
      var list = ComponentModelCollection
        .Where (p => p.Id.Equals (id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        return (list [0]);
      }

      return (TComponentModelItem.CreateDefault);
    }
    #endregion

    #region Static
    public static TStyleComponentModel CreateDefault => new TStyleComponentModel (); 
    #endregion
  };
  //---------------------------//

}  // namespace