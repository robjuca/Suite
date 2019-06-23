/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Windows.Media.Imaging;

using rr.Library.Helper;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Gadget.Image
{
  public class TComponentControlModel
  {
    #region Property
    public Collection<BitmapImage> Frames
    {
      get;
      private set;
    }

    public bool IsEmpty
    {
      get
      {
        return (Frames.Count.Equals (0));
      }
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      Frames = new Collection<BitmapImage> ();
      FrameCollection = new Dictionary<Guid, TComponentModelItem> ();
    }
    #endregion

    #region Members
    public void SelectModel (TComponentModelItem modelItem)
    {
      if (modelItem.NotNull ()) {
        if (modelItem.Id.NotEmpty ()) {
          if (modelItem.ImageModel.Image.NotNull ()) {
            FrameCollection.Add (modelItem.Id, modelItem);

            ReOrder ();
          }
        }
      }
    }

    public void Remove (Guid id)
    {
      if (id.NotEmpty ()) {
        if (FrameCollection.ContainsKey (id)) {
          FrameCollection.Remove (id);

          Frames.Clear ();

          foreach (var item in FrameCollection) {
            Frames.Add (THelper.ByteArrayToBitmapImage (item.Value.ImageModel.Image));
          }
        }
      }
    }

    public void RequestComponentModel (List<TComponentModelItem> models)
    {
      if (models.NotNull ()) {
        models.Clear ();

        var orderList = FrameCollection
          .OrderBy (p => p.Value.NodeModel.Position)
          .ToList ()
        ;

        foreach (var item in orderList) {
          models.Add (item.Value);
        }
      }
    }

    public void RequestNodeModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        action.CollectionAction.ExtensionNodeCollection.Clear ();

        int position = 0;

        foreach (var item in FrameCollection) {
          // node
          var nodeModel = Server.Models.Component.ExtensionNode.CreateDefault;
          nodeModel.ChildId = item.Key;
          nodeModel.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Image);
          nodeModel.Position = position.ToString ();

          action.CollectionAction.ExtensionNodeCollection.Add (nodeModel);

          position++;
        }
      }
    }

    public void ReOrder ()
    {
      var models = FrameCollection
        .OrderBy (p => p.Value.NodeModel.Position)
        .ToList ()
      ;

      Frames.Clear ();

      foreach (var item in models) {
        Frames.Add (THelper.ByteArrayToBitmapImage (item.Value.ImageModel.Image));
      }
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        Frames.Clear ();

        foreach (var item in alias.Frames) {
          Frames.Add (item.Clone ());
        }
      }
    }

    public void Cleanup ()
    {
      Frames.Clear ();
      FrameCollection.Clear ();
    }
    #endregion

    #region Fields
    Dictionary<Guid, TComponentModelItem> FrameCollection
    {
      get;
      set;
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace