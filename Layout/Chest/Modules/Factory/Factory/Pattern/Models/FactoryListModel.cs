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
  public class TFactoryListModel
  {
    #region Property
    public ObservableCollection<TComponentItemInfo> InputItemsCollection
    {
      get;
    }

    public ObservableCollection<TComponentItemInfo> OutputItemsCollection
    {
      get;
    }

    public string InputCount
    {
      get
      {
        return ($"[ {InputItemsCollection.Count} ]");
      }
    }

    public string OutputCount
    {
      get
      {
        return ($"[ {OutputItemsCollection.Count} ]");
      }
    }
    #endregion

    #region Constructor
    public TFactoryListModel ()
    {
      InputItemsCollection = new ObservableCollection<TComponentItemInfo> ();
      OutputItemsCollection = new ObservableCollection<TComponentItemInfo> ();

      m_ComponentItems = new Collection<Guid> ();
    }
    #endregion

    #region Members
    internal void SelectComponentRelation (TEntityAction action)
    {
      action.ThrowNull ();

      // category
      if (action.CollectionAction.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
        int childCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Drawer);

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

      InputItemsCollection.Clear ();

      foreach (var modelItem in action.CollectionAction.ModelCollection) {
        var id = modelItem.Key;
        var modelAction = modelItem.Value;

        var componentModel = TComponentModel.Create (modelAction);
        var componentModelItem = TComponentModelItem.Create (componentModel);
        componentModelItem.Select (action.CategoryType.Category);

        var itemInfo = TComponentItemInfo.Create (componentModelItem);

        InputItemsCollection.Add (itemInfo);
      }
    }

    internal void SelectModel (TEntityAction action)
    {
      //action.Id = ComponentModelItem.Id; //  Id
      //action.ModelAction.CopyFrom (ComponentModelItem.RequestModel ()); //  model
      //action.Param1 = new Collection<TComponentModelItem> (ComponentModelItemCollection.ToList ()); // child collection

      action.ThrowNull ();

      if (action.Param1 is Collection<TComponentModelItem> list) {
        foreach (var item in list.OrderBy (p => p.GeometryModel.PositionIndex)) {
          var info = TComponentItemInfo.Create (item);

          OutputItemsCollection.Add (info); // add to output
        }
      }
    }

    internal void RequestRelations (TEntityAction action)
    {
      action.ThrowNull ();

      action.IdCollection.Clear ();

      foreach (var item in m_ComponentItems) {
        action.IdCollection.Add (item);
      }
    }

    public void RequestModel (TEntityAction action)
    {
      // action.CategoryType.Category ParentCategory

      action.ThrowNull ();

      for (int index = 0; index < OutputItemsCollection.Count; index++) {
        var output = OutputItemsCollection [index];

        var relation = ComponentRelation.CreateDefault;
        relation.ChildId = output.Id;
        relation.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (output.Category);
        relation.PositionIndex = index;
        relation.ParentCategory = Server.Models.Infrastructure.TCategoryType.ToValue (action.CategoryType.Category);

        action.CollectionAction.ComponentRelationCollection.Add (relation);
      }
    }

    public Dictionary <Guid, int> RequestOutputPosition (Guid id)
    {
      var list = new Dictionary<Guid, int> ();

      for (int position = 0; position < OutputItemsCollection.Count; position++) {
        if (id.Equals (OutputItemsCollection [position].Id)) {
          list.Add (id, position);
          break;
        }
      }

      return (list);
    }

    internal void DropToInput (TComponentItemInfo item)
    {
      item.ThrowNull ();

      // remove from output
      var list = OutputItemsCollection
        .Where (p => p.Id.Equals (item.Id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        OutputItemsCollection.Remove (list [0]);
      }

      // add to input
      InputItemsCollection.Add (item);

      // sort
      SortCollection ();
    }

    internal void DropToOutput (TComponentItemInfo item)
    {
      item.ThrowNull ();

      // remove from input
      var list = InputItemsCollection
        .Where (p => p.Id.Equals (item.Id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        InputItemsCollection.Remove (list [0]);
      }

      // add to output
      OutputItemsCollection.Add (item);
    }

    internal void Cleanup ()
    {
      InputItemsCollection.Clear ();
      OutputItemsCollection.Clear ();

      m_ComponentItems.Clear ();
    }
    #endregion

    #region Fields
    readonly Collection<Guid>                                   m_ComponentItems;
    #endregion

    #region Support
    void SortCollection ()
    {
      var list = new List<TComponentItemInfo> (InputItemsCollection);

      InputItemsCollection.Clear ();

      foreach (var item in list.OrderBy (p => p.Name)) {
        InputItemsCollection.Add (item);
      }
    } 
    #endregion
  };
  //---------------------------//
  
}  // namespace
