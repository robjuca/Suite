/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Component;

using Shared.ViewModel;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryComponentListModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> ComponentSourceCollection
    {
      get;
      private set;
    }

    public int ComponentCount
    {
      get
      {
        return (ComponentSourceCollection.Count);
      }
    }
    #endregion

    #region Constructor
    public TFactoryComponentListModel ()
    {
      ComponentSourceCollection = new ObservableCollection<TComponentModelItem> ();

      m_ComponentRemovedItems = new Dictionary<Guid, TComponentModelItem> ();
      m_ComponentTryToInsertItems = new Dictionary<Guid, TComponentModelItem> ();

      m_ComponentItems = new Collection<Guid> ();

      m_NodeCollection = new Collection<ExtensionNode> ();
    }
    #endregion

    #region Members
    #region Select
    internal void SelectComponentRelation (TEntityAction action)
    {
      action.ThrowNull ();

      // category
      if (action.CollectionAction.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
        int childCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Shelf);

        // parent
        foreach (var item in action.ComponentOperation.ParentCategoryCollection) {
          foreach (var model in item.Value) {
            if (model.ChildCategory.Equals (childCategory)) {
              m_ComponentItems.Add (model.ChildId);
            }
          }
        }
      }
    }

    internal void Select (TEntityAction action)
    {
      // action.CollectionAction.ModelCollection [id]{model}
      // action.CollectionAction.ExtensionNodeCollection [id]{model}

      action.ThrowNull ();

      m_NodeCollection = new Collection<ExtensionNode> (action.CollectionAction.ExtensionNodeCollection);

      ComponentSourceCollection.Clear ();

      foreach (var modelItem in action.CollectionAction.ModelCollection) {
        var id = modelItem.Key;
        var modelAction = modelItem.Value;

        IList<ExtensionNode> list = m_NodeCollection
          .Where (p => p.ParentId.Equals (id))
          .ToList ()
        ;

        var componentModel = TComponentModel.Create (modelAction);
        var componentModelItem = TComponentModelItem.Create (componentModel);
        componentModelItem.Select (list);
        componentModelItem.Select (action.CategoryType.Category);

        if (m_ComponentRemovedItems.ContainsKey (componentModelItem.Id).IsFalse ()) {
          ComponentSourceCollection.Add (componentModelItem);
        }
      }

      // try to insert
      foreach (var component in m_ComponentTryToInsertItems) {
        var itemInfo = component.Value;
        ComponentSourceCollection.Add (itemInfo);
      }

      SortCollection ();
    }

    internal void SelectDefault ()
    {
      m_ComponentItems.Clear ();
      m_ComponentRemovedItems.Clear ();
      m_ComponentTryToInsertItems.Clear ();
    }
    #endregion

    #region Request
    internal void RequestRelations (TEntityAction action)
    {
      action.ThrowNull ();

      action.IdCollection.Clear ();

      foreach (var item in m_ComponentItems) {
        action.IdCollection.Add (item);
      }
    }
    #endregion

    #region Misc
    internal void Drop (Guid id)
    {
      foreach (var item in ComponentSourceCollection) {
        if (item.Id.Equals (id)) {
          m_ComponentRemovedItems.Add (id, item);
          break;
        }
      }

      var listSource = ComponentSourceCollection
        .Where (p => p.Id.Equals (id))
        .ToList ()
      ;

      if (listSource.Count.Equals (1)) {
        ComponentSourceCollection.Remove (listSource [0]);
      }

      var list = m_ComponentTryToInsertItems
        .Where (p => p.Key.Equals (id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        m_ComponentTryToInsertItems.Remove (list [0].Key);
      }
    }

    internal bool Restore (Guid id)
    {
      var idToRemove = Guid.Empty;

      foreach (var item in m_ComponentRemovedItems) {
        if (item.Key.Equals (id)) {
          var model = item.Value;
          idToRemove = model.Id;
          break;
        }
      }

      if (idToRemove.NotEmpty ()) {
        ComponentSourceCollection.Add (m_ComponentRemovedItems [id]); // restore
        m_ComponentRemovedItems.Remove (idToRemove);

        SortCollection ();

        return (true);
      }

      return (false);
    }

    internal Guid TryToInsert (TEntityAction action)
    {
      /*
       - action.ModelAction (model)
       - action.CollectionAction.ExtensionNodeCollection 
       - action.CollectionAction.ModelCollection {id, model} node
      */

      action.ThrowNull ();

      var componentModel = TComponentModel.Create (action.ModelAction);

      var componentModelItem = TComponentModelItem.Create (componentModel);
      componentModelItem.Select (action.CategoryType.Category);

      // nodes
      foreach (var item in action.CollectionAction.EntityCollection) {
        componentModelItem.Select (item.Value.CollectionAction.ExtensionNodeCollection);
      }

      m_ComponentTryToInsertItems.Add (componentModelItem.Id, componentModelItem);

      // try to insert
      ComponentSourceCollection.Add (componentModelItem);
      SortCollection ();

      return (componentModelItem.Id);
    }
    #endregion
    #endregion

    #region Fields
    readonly Dictionary<Guid, TComponentModelItem>                        m_ComponentRemovedItems;
    readonly Dictionary<Guid, TComponentModelItem>                        m_ComponentTryToInsertItems;
    readonly Collection<Guid>                                             m_ComponentItems;
    Collection<ExtensionNode>                                             m_NodeCollection;
    #endregion

    #region Support
    bool IsRemovedItem (Guid id)
    {
      foreach (var item in m_ComponentRemovedItems) {
        if (item.Key.Equals (id)) {
          return (true);
        }
      }

      return (false);
    }

    bool IsContentItem (Guid id)
    {
      foreach (var item in m_ComponentItems) {
        if (item.Equals (id)) {
          return (true);
        }
      }

      return (false);
    }

    void SortCollection ()
    {
      var list = new List<TComponentModelItem> (ComponentSourceCollection);

      ComponentSourceCollection.Clear ();
      ComponentSourceCollection = new ObservableCollection<TComponentModelItem> (list.OrderBy (p => p.Name).ToList ());
    }
    #endregion
  };
  //---------------------------//

}  // namespace
