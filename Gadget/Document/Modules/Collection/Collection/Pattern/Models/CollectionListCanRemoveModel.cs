/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Module.Collection.Pattern.Models
{
  public class TCollectionListCanRemoveModel
  {
    #region Property
    public ObservableCollection<TComponentModelItem> ItemsCollection
    {
      get; 
      private set;
    }

    public TStyleSelectorModel StyleSelectorModel
    {
      get;
      private set;
    }

    public string StyleSelectorSelect
    {
      get;
      set;
    }

    public string StyleInfo
    {
      get
      {
        return ($"{StyleSelectorModel.Current.Style} [ {StyleSelectorModel.Current.ItemsCollection.Count} ]");
      }
    }

    public string MessagePanel
    {
      get;
      set;
    }

    public bool IsActiveProgress
    {
      get;
      set;
    }

    public string Question
    {
      get
      {
        return ($"Removing [150] items...");
      }
    }
    #endregion

    #region Constructor
    public TCollectionListCanRemoveModel ()
    {
      ItemsCollection = new ObservableCollection<TComponentModelItem> ();

      StyleSelectorModel = TStyleSelectorModel.CreateDefault;
    }
    #endregion

    #region Members
    internal bool Select (Server.Models.Component.TEntityAction action, Collection<Guid> idList)
    {
      if (action != null) {
        foreach (var id in action.IdCollection) {
          var list = action.CollectionAction.ExtensionLayoutCollection
            .Where (p => p.Id.Equals (id))
            .ToList ()
          ;

          //if (list.Count.Equals (1)) {
          //  //check busy (disable  and not busy)
          //  if (list [0].Locked) {
          //    continue;
          //  }

          //  //add (disable and not busy)
          //  else {
          //    idList.Add (id);
          //  }
          //}

          //// add (just disable)
          //else {
          //  idList.Add (id);
          //}
        }
      }

      return (idList.Count > 0);
    }

    internal void Select (Server.Models.Component.TEntityAction action)
    {
      if (action != null) {
        // action.CollectionAction.ModelCollection (Document collection)
        StyleSelectorModel.SelectItem (action);

        StyleSelectorSelect = TContentStyle.MINI;
      }
    }

    internal void SelectStyle (TContentStyle.Style selectedStyle)
    {
      StyleSelectorModel.Select (selectedStyle);

      ItemsCollection = new ObservableCollection<TComponentModelItem> (StyleSelectorModel.Request (selectedStyle).ItemsCollection);
    }

    internal void ShowPanels ()
    {
      IsActiveProgress = true;
      MessagePanel = "applying...";
    }

    internal void ClearPanels ()
    {
      IsActiveProgress = false;
      MessagePanel = string.Empty;
    }

    internal void SelectAll ()
    {
      
    }

    internal void UnselectAll ()
    {

    }
    #endregion
  };
  //---------------------------//

}  // namespace
