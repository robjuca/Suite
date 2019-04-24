﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
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

      m_ComponentModelDrop = new Dictionary<Guid, TComponentModelItem> ();
    }
    #endregion

    #region Members
    public void Select (Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection
      // action.CollectionAction.ExtensionNodeCollection

      m_ComponentModelDrop.Clear ();

      if (action.NotNull ()) {
        ComponentModelCollection.Clear ();
        ItemsCollection.Clear ();

        foreach (var modelAction in action.CollectionAction.ModelCollection) {
          var componentModel = Server.Models.Component.TComponentModel.Create (modelAction.Value);

          var componentModelItem = TComponentModelItem.Create (componentModel);
          componentModelItem.Select (action.CategoryType.Category); 

          ComponentModelCollection.Add (componentModelItem);
        }

        // sort collection by Name
        var sortedList = ComponentModelCollection
          .OrderBy (p => p.Name)
          .ToList ()
        ;

        ComponentModelCollection.Clear ();

        foreach (var item in sortedList) {
          ComponentModelCollection.Add (item);
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

        // zap drop model
        foreach (var item in m_ComponentModelDrop) {
          var dropList = list
            .Where (p => p.Id.Equals (item.Key))
            .ToList ()
          ;

          if (dropList.Count.Equals (1)) {
            var model = dropList [0];
            list.Remove (model);
          }
        }

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

    public bool DropComponentModel (Guid id)
    {
      var res = false;

      var list = ComponentModelCollection
        .Where (p => p.Id.Equals (id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        var model = list [0];
        m_ComponentModelDrop.Add (model.Id, model);
        res = true;
      }

      return (res);
    }

    public bool RestoreComponentModel (Guid id)
    {
      var res = false;

      if (m_ComponentModelDrop.ContainsKey (id)) {
        m_ComponentModelDrop.Remove (id);
        res = true;
      }

      return (res);
    }
    #endregion

    #region Fields
    readonly Dictionary<Guid, TComponentModelItem>                        m_ComponentModelDrop; 
    #endregion

    #region Static
    public static TStyleComponentModel CreateDefault => new TStyleComponentModel (); 
    #endregion
  };
  //---------------------------//

}  // namespace