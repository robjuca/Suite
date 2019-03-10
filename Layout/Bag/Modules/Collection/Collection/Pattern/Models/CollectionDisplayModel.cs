/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Windows;

using Server.Models.Component;

using Shared.ViewModel;

using Shared.Layout.Bag;
//---------------------------//

namespace Layout.Collection.Pattern.Models
{
  public sealed class TCollectionDisplayModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      private  set;
    }

    public string Style
    {
      get;
      set;
    }

    public string ChildStyle
    {
      get;
      set;
    }

    public string Name
    {
      get;
      private set;
    }

    public string ChildName
    {
      get;
      private set;
    }

    public string ComponentCount
    {
      get
      {
        return (m_Count.ToString ());
      }
    }

    public bool IsEditCommandEnabled
    {
      get;
      set;
    }

    public bool IsRemoveCommandEnabled
    {
      get;
      set;
    }

    public Visibility DocumentVisibility
    {
      get
      {
        return (ComponentControlModel.DocumentVisibility);
      }
    }

    public Visibility ImageVisibility
    {
      get
      {
        return (ComponentControlModel.ImageVisibility);
      }
    }

    public Visibility BusyVisibility
    {
      get
      {
        return (ComponentModelItem.BusyVisibility);
      }
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get
      {
        return (ComponentModelItem.Category);
      }
    }

    public Server.Models.Infrastructure.TCategory ChildCategory
    {
      get;
      private set;
    }

    public TComponentModelItem ComponentModelItem
    {
      get;
      private set;
    }

    public Guid Id
    {
      get
      {
        return (ComponentModelItem.Id);
      }
    }

    public Guid ChildId
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;

      ComponentModelItem = TComponentModelItem.CreateDefault;

      IsEditCommandEnabled = false;
      IsRemoveCommandEnabled = false;

      m_Count = 0;
      Name = string.Empty;

      m_ModelItems = new Dictionary<Guid, TComponentModelItem> ();
    }
    #endregion

    #region Members
    internal void StyleChanged (TComponentModelItem model)
    {
      SelectStyle (model);
    }

    internal void SelectModel (TEntityAction action)
    {
      /*
       DATA IN:
        - action.ModelAction {bag model} or action.ComponentModel
        - action.CollectionAction.ExtensionNodeCollection 
        - action.CollectionAction.ModelCollection {id, model}
      */

      action.ThrowNull ();

      m_Count = 0;
      m_ModelItems.Clear ();

      ComponentModelItem.CopyFrom (TComponentModelItem.Create (action.ComponentModel));
      ComponentModelItem.Select (action.CategoryType.Category);

      Name = ComponentModelItem.Name;

      foreach (var models in action.CollectionAction.ModelCollection) {
        var modelAction = models.Value;

        ChildId = models.Key;
        ChildCategory = Server.Models.Infrastructure.TCategoryType.FromValue (modelAction.ExtensionNodeModel.ChildCategory);
        //TODO: review
        //ChildStyle = modelAction.ExtensionLayoutModel.Style;

        var modelBase = TComponentModel.Create (modelAction);

        var childModel = TComponentModelItem.Create (modelBase);
        childModel.Select (ChildCategory);

        Select (childModel);

        m_ModelItems.Add (ChildId, childModel);
      }

      IsEditCommandEnabled = true;
      IsRemoveCommandEnabled = ComponentModelItem.CanRemove && IsEmpty;
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      action.SelectModel (ComponentModelItem);

      action.CollectionAction.ModelCollection.Clear ();

      foreach (var item in m_ModelItems) {
        action.CollectionAction.ModelCollection.Add (item.Key, item.Value.RequestModel ());
      }
    }

    internal void Cleanup ()
    {
      ComponentModelItem = TComponentModelItem.CreateDefault;

      ComponentControlModel.Cleanup ();

      ChildCategory = Server.Models.Infrastructure.TCategory.None;
      m_Count = 0;

      Name = string.Empty;

      IsEditCommandEnabled = false;
      IsRemoveCommandEnabled = false;

      m_ModelItems.Clear ();
    }
    #endregion

    #region Property
    bool IsEmpty
    {
      get
      {
        return (m_Count.Equals (0));
      }
    } 
    #endregion

    #region Fields
    Dictionary<Guid, TComponentModelItem>             m_ModelItems;
    int                                               m_Count;
    #endregion

    #region Support
    void SelectStyle (TComponentModelItem model)
    {
      //TODO: review
      //if (string.IsNullOrEmpty (model.LayoutModel.Style)) {
      //  Cleanup ();
      //}

      //else {
      //  ComponentControlModel.Cleanup ();

      //  var width = model.LayoutModel.Width;
      //  var height = model.LayoutModel.Height;

      //  //TODO: review
      //  //Style = $"[ style: {width} x {height} - {model.LayoutModel.Style} ]";

      //  m_Count = 0;
      //}
    }

    void Select (TComponentModelItem childModel)
    {
      childModel.ThrowNull ();

      ComponentControlModel.SelectModel (Id, Category);
      ComponentControlModel.SelectChildModel (ChildId, ChildCategory, ChildStyle, childModel);

      switch (ChildCategory) {
        case Server.Models.Infrastructure.TCategory.Document:
          m_Count = 1;
          break;

        case Server.Models.Infrastructure.TCategory.Image:
          m_Count++;
          break;
      }
    }
    #endregion    
  };
  //---------------------------//

}  // namespace
