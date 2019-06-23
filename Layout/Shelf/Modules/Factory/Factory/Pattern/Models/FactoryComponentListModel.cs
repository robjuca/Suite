/*----------------------------------------------------------------
Copyright (C) 2001 R&R Soft - All rights reserved.
author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;

using Server.Models.Component;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryComponentListModel
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

    public string StyleSelectorString
    {
      get
      {
        return ($"{StyleHorizontalSelectorSelect}, {StyleVerticalSelectorSelect}");
      }
    }

    public int ComponentCount
    {
      get
      {
        return (StyleComponentModel.ItemsCount);
      }
    }

    public int SlideIndex
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryComponentListModel ()
    {
      StyleComponentModel = TStyleComponentModel.CreateDefault;

      StyleHorizontalSelectorModel = TStyleSelectorModel.Create (TContentStyle.Mode.Horizontal);
      StyleHorizontalSelectorSelect = string.Empty;

      StyleVerticalSelectorModel = TStyleSelectorModel.Create (TContentStyle.Mode.Vertical);
      StyleVerticalSelectorSelect = string.Empty;

      m_ComponentItems = new Collection<Guid> ();

      m_SelectedStyleHorizontal = TContentStyle.Style.mini;
      m_SelectedStyleVertical = TContentStyle.Style.mini;

      SlideIndex = 0;
    }
    #endregion

    #region Members
    #region Select
    internal void SelectComponentRelation (TEntityAction action)
    {
      action.ThrowNull ();

      // category
      if (action.CollectionAction.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
        int bagCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Bag);

        // parent
        foreach (var item in action.ComponentOperation.ParentCategoryCollection) {
          foreach (var model in item.Value) {
            if (model.ChildCategory.Equals (bagCategory)) {
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

      StyleHorizontalSelectorSelect = string.IsNullOrEmpty (StyleHorizontalSelectorSelect) ? TContentStyle.MINI : StyleHorizontalSelectorSelect;
      StyleVerticalSelectorSelect = string.IsNullOrEmpty (StyleVerticalSelectorSelect) ? TContentStyle.MINI : StyleVerticalSelectorSelect;

      SelectStyle (m_SelectedStyleHorizontal, m_SelectedStyleVertical, action);
    }

    internal void SelectStyleHorizontal (TContentStyle.Style selectedStyleHorizontal)
    {
      StyleHorizontalSelectorModel.Select (selectedStyleHorizontal);
      StyleHorizontalSelectorSelect = selectedStyleHorizontal.ToString ();
      m_SelectedStyleHorizontal = selectedStyleHorizontal;

      Populate ();
    }

    internal void SelectStyleVertical (TContentStyle.Style selectedStyleVertical)
    {
      StyleVerticalSelectorModel.Select (selectedStyleVertical);
      StyleVerticalSelectorSelect = selectedStyleVertical.ToString ();
      m_SelectedStyleVertical = selectedStyleVertical;

      Populate ();
    }

    internal void SelectStyle (TContentStyle.Style selectedStyleHorizontal, TContentStyle.Style selectedStyleVertical, Server.Models.Component.TEntityAction action)
    {
      // DATA IN:
      // action.CollectionAction.ModelCollection

      StyleComponentModel.Select (action);

      SelectStyleHorizontal (selectedStyleHorizontal);
      SelectStyleVertical (selectedStyleVertical);
    }

    internal void SelectDefault ()
    {
      m_ComponentItems.Clear ();

      StyleComponentModel.Cleanup ();

      Populate ();
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
    internal bool DropComponentModel (Guid id)
    {
      var res = StyleComponentModel.DropComponentModel (id);

      Populate ();

      return (res);
    }

    internal bool RestoreComponentModel (Guid id)
    {
      var res = StyleComponentModel.RestoreComponentModel (id);

      Populate ();

      return (res);
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
      componentModelItem.Select (action.CollectionAction.ExtensionNodeCollection);
      componentModelItem.Select (action.CategoryType.Category);

      StyleComponentModel.TryToInsert (componentModelItem);

      Populate ();

      return (componentModelItem.Id);
    }
    #endregion
    #endregion

    #region Fields
    readonly Collection<Guid>                                             m_ComponentItems;
    TContentStyle.Style m_SelectedStyleHorizontal;
    TContentStyle.Style m_SelectedStyleVertical;
    #endregion

    #region Support
    void Populate ()
    {
      StyleComponentModel.Select (m_SelectedStyleHorizontal, m_SelectedStyleVertical);
    }
    #endregion
  };
  //---------------------------//

}  // namespace
