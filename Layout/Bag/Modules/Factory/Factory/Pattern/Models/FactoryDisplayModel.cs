/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using Server.Models.Component;

using Shared.ViewModel;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryDisplayModel
  {
    #region Property
    public Shared.Gadget.Document.TComponentControlModel ComponentDocumentControlModel
    {
      get;
      set;
    }

    public Shared.Gadget.Image.TComponentControlModel ComponentImageControlModel
    {
      get;
      set;
    }

    public string Style
    {
      get;
      set;
    }

    public string ComponentCount
    {
      get
      {
        return (m_Count.ToString ());
      }
    }

    public Visibility DocumentVisibility
    {
      get;
      set;
    }

    public Visibility ImageVisibility
    {
      get;
      set;
    }

    public bool IsEnabledOrder
    {
      get
      {
        return (ComponentImageControlModel.IsEmpty.IsFalse ());
      }
    }

    public ObservableCollection<TComponentModelItem> OrderFrameItemsSource
    {
      get;
      set;
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get; 
      private set;
    }
    #endregion

    #region Constructor
    public TFactoryDisplayModel ()
    {
      ComponentDocumentControlModel = Shared.Gadget.Document.TComponentControlModel.CreateDefault;
      ComponentImageControlModel = Shared.Gadget.Image.TComponentControlModel.CreateDefault;

      DocumentVisibility = Visibility.Collapsed;
      ImageVisibility = Visibility.Collapsed;

      OrderFrameItemsSource = new ObservableCollection<TComponentModelItem> ();

      Category = Server.Models.Infrastructure.TCategory.None;

      m_Count = 0;
    }
    #endregion

    #region Members
    internal void StyleChanged (TEntityAction action)
    {
      SelectStyle (action);
    }

    internal void Modify (Server.Models.Infrastructure.TCategory category)
    {
      category.ThrowNull ();

      Category = category;

      DocumentVisibility = Visibility.Collapsed;
      ImageVisibility = Visibility.Collapsed;

      ComponentDocumentControlModel.Cleanup ();
      ComponentImageControlModel.Cleanup ();

      switch (category) {
        case Server.Models.Infrastructure.TCategory.Document:
          DocumentVisibility = Visibility.Visible;
          break;

        case Server.Models.Infrastructure.TCategory.Image:
          ImageVisibility = Visibility.Visible;
          break;
      }

      m_Count = 0;
    }

    internal void Select (Server.Models.Infrastructure.TCategory category, TComponentModelItem model)
    {
      category.ThrowNull ();
      model.ThrowNull ();

      model.Select (category);

      // document
      if (category.Equals (Server.Models.Infrastructure.TCategory.Document)) {
        SelectDocumentModel (model);
      }

      // image
      if (category.Equals (Server.Models.Infrastructure.TCategory.Image)) {
        SelectImageModel (model);
      }
    }

    internal void Remove (Server.Models.Infrastructure.TCategory category, TComponentModelItem model)
    {
      category.ThrowNull ();
      model.ThrowNull ();

      // image
      if (category.Equals (Server.Models.Infrastructure.TCategory.Image)) {
        ComponentImageControlModel.Remove (model.Id);
        m_Count--;
      }
    }

    internal void RequestModel (TEntityAction action)
    {
      action.ThrowNull ();

      switch (Category) {
        case Server.Models.Infrastructure.TCategory.Document: {
            if (ComponentDocumentControlModel.Id.NotEmpty ()) {
              ComponentDocumentControlModel.RequestNodeModel (action);

              // status
              var statusModel = ComponentStatus.CreateDefault;
              statusModel.Id = ComponentDocumentControlModel.Id;
              statusModel.Busy = true;

              action.CollectionAction.ComponentStatusCollection.Add (statusModel);
            }
          }
          break;

        case Server.Models.Infrastructure.TCategory.Image: {
            ComponentImageControlModel.RequestNodeModel (action);

            // order
            for (int index = 0; index < OrderFrameItemsSource.Count; index++) {
              var item = OrderFrameItemsSource [index];

              var list = action.CollectionAction.ExtensionNodeCollection
                .Where (p => p.ChildId.Equals (item.Id))
                .ToList ()
              ;

              // found
              if (list.Count.Equals (1)) {
                list [0].Position = index.ToString ();
              }
            }

            // status
            foreach (var item in action.CollectionAction.ExtensionNodeCollection) {
              var statusModel = ComponentStatus.CreateDefault;
              statusModel.Id = item.ChildId;
              statusModel.Busy = true;

              action.CollectionAction.ComponentStatusCollection.Add (statusModel);
            }
          }
          break;
      }
    }

    internal void RequestOrder ()
    {
      OrderFrameItemsSource.Clear ();

      var list = new List<TComponentModelItem> ();

      ComponentImageControlModel.Request (list);

      foreach (var item in list) {
        OrderFrameItemsSource.Add (item);
      }
    }

    internal void ReOrder ()
    {
      ComponentImageControlModel.Cleanup ();
      m_Count = 0;

      for (int index = 0; index < OrderFrameItemsSource.Count; index++) {
        OrderFrameItemsSource [index].NodeModel.Position = index.ToString ();
      }

      var list = OrderFrameItemsSource
        .OrderBy (p => p.NodeModel.Position)
        .ToList ()
      ;

      foreach (var model in list) {
        SelectImageModel (model);
      }
    }

    internal void Cleanup ()
    {
      ComponentDocumentControlModel.Cleanup ();
      ComponentImageControlModel.Cleanup ();

      OrderFrameItemsSource.Clear ();

      Category = Server.Models.Infrastructure.TCategory.None;
      m_Count = 0;
      Style = string.Empty;

      DocumentVisibility = Visibility.Collapsed;
      ImageVisibility = Visibility.Collapsed;
    }
    #endregion

    #region Fields
    int                                     m_Count; 
    #endregion

    #region Support
    void SelectStyle (TEntityAction action)
    {
      var width = action.ModelAction.ExtensionLayoutModel.Width;
      var height = action.ModelAction.ExtensionLayoutModel.Height;

      //TODO: review
      //Style = $"[ style: {width} x {height} - {action.ModelAction.ExtensionLayoutModel.Style} ]";

      ComponentImageControlModel.Cleanup ();

      m_Count = 0;
    }

    void SelectDocumentModel (TComponentModelItem model)
    {
      ComponentDocumentControlModel.Id = model.Id;

      ComponentDocumentControlModel.RtfHeader = model.DocumentModel.Header;
      ComponentDocumentControlModel.RtfFooter = model.DocumentModel.Footer;
      ComponentDocumentControlModel.RtfParagraph = model.DocumentModel.Paragraph;

      ComponentDocumentControlModel.ExternalLink = model.TextModel.ExternalLink;
      //TODO: review
      //ComponentDocumentControlModel.Style = model.LayoutModel.Style;
      ComponentDocumentControlModel.Width = model.LayoutModel.Width;
      ComponentDocumentControlModel.Height = model.LayoutModel.Height;

      ComponentDocumentControlModel.HeaderVisibility = model.DocumentModel.HeaderVisibility;
      ComponentDocumentControlModel.FooterVisibility = model.DocumentModel.FooterVisibility;

      ComponentDocumentControlModel.ImageGeometry.Position.Position = model.GeometryModel.PositionImage;
      ComponentDocumentControlModel.ImageGeometry.Size.Width = model.ImageModel.Width;
      ComponentDocumentControlModel.ImageGeometry.Size.Height = model.ImageModel.Height;
      ComponentDocumentControlModel.ImageDistorted = model.ImageModel.Distorted;
      ComponentDocumentControlModel.Image = model.ImageModel.Image;

      ComponentDocumentControlModel.PropertyName = "all";

      m_Count = 1;
    }

    void SelectImageModel (TComponentModelItem model)
    {
      ComponentImageControlModel.SelectModel (model);

      m_Count++;
    }
    #endregion
  };
  //---------------------------//

}  // namespace
