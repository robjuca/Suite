/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;

using Shared.ViewModel;

using Shared.Gadget.Document;
//---------------------------//

namespace Gadget.Collection.Pattern.Models
{
  public sealed class TCollectionDisplayModel
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

    public bool IsViewEnabled
    {
      get;
      set;
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
        return (ComponentModelItem.IsNull () ? false : (ComponentModelItem.InfoModel.Enabled.IsFalse ()));
      }
    }

    public Visibility DistortedVisibility
    {
      get;
      set;
    }

    public Visibility BusyVisibility
    {
      get;
      set;
    }

    public bool Distorted
    {
      get
      {
        return (ComponentControlModel.ImageDistorted);
      }
    }

    public Guid Id
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
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
      DistortedVisibility = Visibility.Hidden;

      IsViewEnabled = true;
    }
    #endregion

    #region Members
    internal void Select (TComponentModelItem item)
    {
      ComponentModelItem = item ?? throw new System.ArgumentNullException (nameof (item));

      SelectModel ();

      BusyVisibility = ComponentModelItem.BusyVisibility;
      DistortedVisibility = Distorted ? Visibility.Visible : Visibility.Hidden;
    }

    internal void RequestModel (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      var modelAction = ComponentModelItem.RequestModel ();
      action.ModelAction.CopyFrom (modelAction);

      action.Id = Id;
      action.CategoryType.Select (ComponentModelItem.Category);

      action.ModelAction.ComponentStatusModel.Locked = true;  // Style always locked
    }

    internal void Cleanup ()
    {
      ComponentModelItem = null;
      ComponentControlModel = TComponentControlModel.CreateDefault;

      BusyVisibility = Visibility.Hidden;
      DistortedVisibility = Visibility.Hidden;
    }
    #endregion

    #region Support
    void SelectModel ()
    {
      ComponentControlModel.RtfHeader = ComponentModelItem.DocumentModel.Header;
      ComponentControlModel.RtfFooter = ComponentModelItem.DocumentModel.Footer;
      ComponentControlModel.RtfParagraph = ComponentModelItem.DocumentModel.Paragraph;

      ComponentControlModel.ExternalLink = ComponentModelItem.TextModel.ExternalLink;
      //TODO: review
      //ComponentControlModel.Style = ComponentModelItem.LayoutModel.Style;
      ComponentControlModel.Width = ComponentModelItem.LayoutModel.Width;
      ComponentControlModel.Height = ComponentModelItem.LayoutModel.Height;
      
      ComponentControlModel.HeaderVisibility = ComponentModelItem.DocumentModel.HeaderVisibility;
      ComponentControlModel.FooterVisibility = ComponentModelItem.DocumentModel.FooterVisibility;

      ComponentControlModel.ImageGeometry.Position.Position = ComponentModelItem.GeometryModel.PositionImage;
      ComponentControlModel.ImageGeometry.Size.Width = ComponentModelItem.ImageModel.Width;
      ComponentControlModel.ImageGeometry.Size.Height = ComponentModelItem.ImageModel.Height;
      ComponentControlModel.ImageDistorted = ComponentModelItem.ImageModel.Distorted;
      ComponentControlModel.Image = ComponentModelItem.ImageModel.Image;

      ComponentControlModel.PropertyName = "all";
    }
    #endregion
  };
  //---------------------------//

}  // namespace
