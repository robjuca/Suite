/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Windows;

using Shared.ViewModel;

using Shared.Layout.Chest;
//---------------------------//

namespace Layout.Collection.Pattern.Models
{
  public class TCollectionDisplayModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }

    public TComponentModelItem ComponentModelItem
    {
      get;
      private set;
    }

    public bool IsEditCommandEnabled
    {
      get
      {
        return (ComponentModelItem.ValidateId && ComponentModelItem.Active.IsFalse ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (ComponentModelItem.CanRemove);
      }
    }

    public bool IsActive
    {
      get;
      set;
    }

    public Visibility ActiveVisibility
    {
      get
      {
        return (ComponentModelItem.Enabled ? Visibility.Visible: Visibility.Collapsed);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      Cleanup ();
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        ComponentModelItem.CopyFrom (item);

        IsActive = ComponentModelItem.Active && ComponentModelItem.Enabled;
      }
    }

    internal void SelectById (Server.Models.Component.TEntityAction action)
    {
      // action.ModelAction { Chest model }
      // action.ComponentOperation.ParentIdCollection [id {Chest}];
      // action.CollectionAction.EntityCollection { id, action } chest relation (Drawer)

      if (ComponentModelItem.Id.Equals (action.Id)) {
        // update
        ComponentModelItem.RequestChild (action);
      }
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      action.Id = ComponentModelItem.Id; //  Id
      action.ModelAction.CopyFrom (ComponentModelItem.RequestModel ()); //  model
      action.Param1 = new Collection<TComponentModelItem> (RequestChildCollection ()); // child collection
    }

    internal Collection<TComponentModelItem> RequestChildCollection ()
    {
      return (new Collection<TComponentModelItem> (ComponentModelItem.ChildCollection)); // child collection
    }

    internal void RequestStatus (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      ComponentModelItem.StatusModel.Active = IsActive;

      action.Id = ComponentModelItem.Id; //  Id
      action.ModelAction.CopyFrom (ComponentModelItem.RequestModel ()); //  model
    }

    internal void Cleanup ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // control model
      ComponentModelItem = TComponentModelItem.CreateDefault; //  Chest model

      IsActive = false;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
