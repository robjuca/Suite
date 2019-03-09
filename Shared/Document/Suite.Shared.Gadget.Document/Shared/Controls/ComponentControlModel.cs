/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

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

    public string Style
    {
      get;
      set;
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

        //TODO: review
        //Style = action.ModelAction.ExtensionLayoutModel.Style;
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
        //TODO: review
        //Style = item.LayoutModel.Style;

        Width = item.LayoutModel.Width;
        Height = item.LayoutModel.Height;

        ImageGeometry.Position.Position = item.GeometryModel.PositionImage;
        ImageGeometry.Size.Width = item.ImageModel.Width;
        ImageGeometry.Size.Height = item.ImageModel.Height;

        Image = item.ImageModel.Image;

        HeaderVisibility = item.DocumentModel.HeaderVisibility;
        FooterVisibility = item.DocumentModel.FooterVisibility;

        RtfHeader = item.DocumentModel.Header;
        RtfFooter = item.DocumentModel.Footer;
        RtfParagraph = item.DocumentModel.Paragraph;

        ExternalLink = item.TextModel.ExternalLink;
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

        Style = alias.Style;
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
      Style = string.Empty;
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
    }

    public static TComponentControlModel Create (TComponentModelItem item)
    {
      var model = CreateDefault;
      model.SelectModel (item);

      return (model);
    }
    #endregion

    #region Static
    public static TComponentControlModel CreateDefault => new TComponentControlModel ();
    #endregion
  };
  //---------------------------//

}  // namespace