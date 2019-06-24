/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.ViewModel;

using Shared.Layout.Drawer;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryDisplayModel
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
    #endregion

    #region Constructor
    public TFactoryDisplayModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // control model
      ComponentModelItem = TComponentModelItem.CreateDefault; // model
    }
    #endregion

    #region Members
    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      //action.ModelAction {model}
      //action.Param1 = Collection<TComponentModelItem> {child collection}

      action.ThrowNull ();

      var model = Server.Models.Component.TComponentModel.Create (action.ModelAction);
      var modelItem = TComponentModelItem.Create (model);
      modelItem.Select (action.CategoryType.Category);

      ComponentControlModel.Select (modelItem);
      ComponentModelItem.CopyFrom (modelItem);

      if (action.Param1 is Collection<TComponentModelItem> childList) {
        ComponentModelItem.ChildCollection.Clear ();

        foreach (var item in childList) {
          ComponentModelItem.ChildCollection.Add (item);
        }
      }
    }

    internal void Cleanup ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // control model
      ComponentModelItem = TComponentModelItem.CreateDefault; // model
    }
    #endregion
  };
  //---------------------------//

}  // namespace