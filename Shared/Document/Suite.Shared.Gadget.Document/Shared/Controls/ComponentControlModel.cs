/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using rr.Library.Types;

using Shared.ViewModel;
//---------------------------//

namespace Shared.Gadget.Document
{
  public class TComponentControlModel
  {
    #region Property
    public string PropertyName
    {
      get;
      set;
    }

    public string RtfHeader
    {
      get;
      set;
    }

    public string RtfFooter
    {
      get;
      set;
    }

    public string RtfParagraph
    {
      get;
      set;
    }

    public string ExternalLink
    {
      get;
      set;
    }

    public string HorizontalStyleString
    {
      get;
      private set;
    }

    public string VerticalStyleString
    {
      get;
      private set;
    }

    public int Width
    {
      get;
      set;
    }

    public int Height
    {
      get;
      set;
    }

    public TGeometry ImageGeometry
    {
      get;
      set;
    }

    public byte [] Image
    {
      get;
      set;
    }

    public bool ImageDistorted
    {
      get;
      set;
    }

    public string HeaderVisibility
    {
      get;
      set;
    }

    public string FooterVisibility
    {
      get;
      set;
    }

    public string InfoReport
    {
      get;
      set;
    }

    public string ImageInfoReport
    {
      get;
      set;
    }

    public Guid Id
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    TComponentControlModel ()
    {
      Cleanup ();
    }
    #endregion

    #region Members
    public void SelectModel (string propertyName, Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        PropertyName = propertyName;

        Id = action.ModelAction.ComponentInfoModel.Id;

        HorizontalStyleString = action.ModelAction.ExtensionLayoutModel.StyleHorizontal;
        VerticalStyleString = action.ModelAction.ExtensionLayoutModel.StyleVertical;
        Width = action.ModelAction.ExtensionLayoutModel.Width;
        Height = action.ModelAction.ExtensionLayoutModel.Height;

        ImageGeometry.Position.Position = action.ModelAction.ExtensionGeometryModel.PositionImage;
        ImageGeometry.Size.Width = action.ModelAction.ExtensionImageModel.Width;
        ImageGeometry.Size.Height = action.ModelAction.ExtensionImageModel.Height;
        Image = action.ModelAction.ExtensionImageModel.Image;
        
        HeaderVisibility = action.ModelAction.ExtensionDocumentModel.HeaderVisibility;
        FooterVisibility = action.ModelAction.ExtensionDocumentModel.FooterVisibility;

        ExternalLink = action.ModelAction.ExtensionDocumentModel.ExternalLink;

        if (propertyName.Equals ("all")) {
          RtfHeader = action.ModelAction.ExtensionDocumentModel.Header;
          RtfFooter = action.ModelAction.ExtensionDocumentModel.Footer;
          RtfParagraph = action.ModelAction.ExtensionDocumentModel.Paragraph;
        }
      }
    }

    public void SelectModel (TComponentModelItem item)
    {
      if (item.NotNull ()) {
        ComponentModelItem.CopyFrom (item);

        Id = item.Id;

        HorizontalStyleString = item.LayoutModel.StyleHorizontal;
        VerticalStyleString = item.LayoutModel.StyleVertical;

        Width = item.LayoutModel.Width;
        Height = item.LayoutModel.Height;

        ImageGeometry.Position.Position = item.GeometryModel.PositionImage;
        ImageGeometry.Size.Width = item.ImageModel.Width;
        ImageGeometry.Size.Height = item.ImageModel.Height;
        ImageDistorted = item.ImageModel.Distorted;
        Image = item.ImageModel.Image;

        HeaderVisibility = item.DocumentModel.HeaderVisibility;
        FooterVisibility = item.DocumentModel.FooterVisibility;

        RtfHeader = item.DocumentModel.Header;
        RtfFooter = item.DocumentModel.Footer;
        RtfParagraph = item.DocumentModel.Paragraph;

        ExternalLink = item.TextModel.ExternalLink;
      }
    }

    public void RequestComponentModel (List<TComponentModelItem> models)
    {
      if (models.NotNull ()) {
        models.Clear ();
        models.Add (ComponentModelItem);
      }
    }

    public void RequestNodeModel (Server.Models.Component.TEntityAction action)
    {
      if (action.NotNull ()) {
        // node
        action.CollectionAction.ExtensionNodeCollection.Clear ();

        var nodeModel = Server.Models.Component.ExtensionNode.CreateDefault;
        nodeModel.ChildId = Id;
        nodeModel.ChildCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Document);
        nodeModel.Position = 0.ToString ();

        action.CollectionAction.ExtensionNodeCollection.Add (nodeModel);
      }
    }

    public void CopyFrom (TComponentControlModel alias)
    {
      if (alias.NotNull ()) {
        PropertyName = alias.PropertyName;

        HorizontalStyleString = alias.HorizontalStyleString;
        VerticalStyleString = alias.VerticalStyleString;
        Width = alias.Width;
        Height = alias.Height;

        ImageGeometry.CopyFrom (alias.ImageGeometry);
        
        Image = alias.Image;

        HeaderVisibility = alias.HeaderVisibility;
        FooterVisibility = alias.FooterVisibility;

        ExternalLink = alias.ExternalLink;

        RtfHeader = alias.RtfHeader;
        RtfFooter = alias.RtfFooter;
        RtfParagraph = alias.RtfParagraph;
      }
    }
    public void Cleanup ()
    {
      PropertyName = string.Empty;
      ExternalLink = string.Empty;
      HorizontalStyleString = string.Empty;
      VerticalStyleString = string.Empty;
      Width = 0;
      Height = 0;
      ImageGeometry = TGeometry.CreateDefault;
      Image = null;
      HeaderVisibility = string.Empty;
      FooterVisibility = string.Empty;
      RtfHeader = string.Empty;
      RtfFooter = string.Empty;
      RtfParagraph = string.Empty;

      ImageDistorted = false;
      ImageInfoReport = string.Empty;
      InfoReport = string.Empty;

      Id = Guid.Empty;

      ComponentModelItem = TComponentModelItem.CreateDefault;
    }
    #endregion

    #region Property
    TComponentModelItem ComponentModelItem
    {
      get;
      set;
    }
    #endregion

    #region Static
    public static TComponentControlModel Create (TComponentModelItem item)
    {
      var model = CreateDefault;
      model.SelectModel (item);

      return (model);
    }
    
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace