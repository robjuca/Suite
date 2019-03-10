/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Server.Models.Component;

using Shared.ViewModel;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public class TCollectionDisplayModel
  {
    #region Property
    public int ImageWidth
    {
      get;
      set;
    }

    public int ImageHeight
    {
      get;
      set;
    }

    public byte [] Image
    {
      get;
      set;
    }

    public string Name
    {
      get
      {
        return (ComponentModelItem.IsNull () ? string.Empty : ComponentModelItem.Name);
      }
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
        return (ComponentModelItem.NotNull ());
      }
    }

    public bool IsRemoveCommandEnabled
    {
      get
      {
        return (ComponentModelItem.IsNull () ? false : ComponentModelItem.CanRemove);
      }
    }

    public Visibility BusyVisibility
    {
      get;
      set;
    }

    public Guid CurrentId
    {
      get
      {
        return (ComponentModelItem.IsNull () ? Guid.Empty : ComponentModelItem.Id);
      }
    }
    #endregion

    #region Constructor
    public TCollectionDisplayModel ()
    {
      BusyVisibility = Visibility.Hidden;
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem componentModelItem)
    {
      ComponentModelItem = componentModelItem ?? throw new ArgumentNullException (nameof (componentModelItem));

      Image = ComponentModelItem.ImageModel.Image;
      ImageWidth = ComponentModelItem.ImageModel.Width;
      ImageHeight = ComponentModelItem.ImageModel.Height;

      BusyVisibility = componentModelItem.BusyVisibility;

      ComponentModelItem.StatusModel.Locked = true; //Image style always locked
    }

    internal void Request (TEntityAction action)
    {
      action.ThrowNull ();
      ComponentModelItem.ThrowNull ();

      action.Id = CurrentId;

      action.ModelAction.ComponentInfoModel.CopyFrom (ComponentModelItem.InfoModel);
      action.ModelAction.ComponentStatusModel.CopyFrom (ComponentModelItem.StatusModel);

      action.ModelAction.ExtensionImageModel.CopyFrom (ComponentModelItem.ImageModel);
      action.ModelAction.ExtensionLayoutModel.CopyFrom (ComponentModelItem.LayoutModel);
      action.ModelAction.ExtensionTextModel.CopyFrom (ComponentModelItem.TextModel);
    }

    internal void Cleanup ()
    {
      ComponentModelItem = null;

      Image = null;
      ImageWidth = 0;
      ImageHeight = 0;

      BusyVisibility = Visibility.Collapsed;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
