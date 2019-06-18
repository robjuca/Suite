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

      m_NodeCollection = new Collection<ExtensionNode> ();

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

      m_NodeCollection = new Collection<ExtensionNode> (action.CollectionAction.ExtensionNodeCollection);

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

    internal void SelectStyle (TContentStyle.Style selectedStyle)
    {
      //TODO:review
      //StyleSelectorModel.Select (selectedStyle);

      //ComponentSourceCollection.Clear ();

      
      //var models = StyleSelectorModel.Request (selectedStyle).ItemsCollection;

      //foreach (var componentModelItem in models) {
      //  IList<ExtensionNode> list = m_NodeCollection
      //    .Where (p => p.ParentId.Equals (componentModelItem.Id))
      //    .ToList ()
      //  ;

      //  componentModelItem.Select (list);

      //  var itemInfo = TComponentItemInfo.Create (componentModelItem);

      //  if (m_ComponentRemovedItems.ContainsKey (itemInfo.Id).IsFalse ()) {
      //    ComponentSourceCollection.Add (itemInfo);
      //  }
      //}

      //m_SelectedStyle = selectedStyle;
      //StyleSelectorSelect = selectedStyle.ToString ();

      // try to insert
      //foreach (var component in m_ComponentTryToInsertItems) {
        //var itemInfo = component.Value;

        //TODO: review
        //if (itemInfo.Style.Equals (StyleSelectorSelect)) {
        //  if (m_ComponentRemovedItems.ContainsKey (itemInfo.Id).IsFalse ()) {
        //    ComponentSourceCollection.Add (itemInfo);
        //  }
        //}
      //}
    }

    internal void SelectDefault ()
    {
      m_ComponentItems.Clear ();

      StyleComponentModel.Cleanup ();

      Populate ();
    }

    //internal void SelectById (Server.Models.Content.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  if (action.CollectionAction.ContentOperation.IsContentOperation (Server.Models.Content.TContentOperation.TInternalOperation.Id)) {
    //    m_ContentSlides = new Collection<Server.Models.Content.ContentLayout> ();
    //    m_ContentBags = new Collection<Server.Models.Content.ContentLayout> ();

    //    foreach (var item in action.CollectionAction.ContentOperation.ContentIdCollection) {
    //      foreach (var model in item.Value) {
    //        switch ((Server.Models.Infrastructure.TContextType.Context) model.ChildType) {
    //          case Server.Models.Infrastructure.TContextType.Context.Bag: {
    //              m_ContentBags.Add (model);
    //            }
    //            break;

    //          case Server.Models.Infrastructure.TContextType.Context.Slide: {
    //              m_ContentSlides.Add (model);
    //            }
    //            break;
    //        }
    //      }
    //    }
    //  }
    //}

    //internal bool SelectMany (Server.Models.Module.Bag.TEntityAction action, Collection<TContentInfo> contentInfoCollection)
    //{
    //  // action.CollectionAction.ModelCollection[BagId] {Bag model}

    //  var res = false;

    //  action.ThrowNull ();
    //  contentInfoCollection.ThrowNull ();

    //  foreach (var item in action.CollectionAction.ModelCollection) {
    //    var bagList = m_ContentBags
    //      .Where (p => p.ChildId.Equals (item.Key))
    //      .ToList ()
    //    ;

    //    if (bagList.Count.Equals (1)) {
    //      var contentBag = bagList [0];
    //      var bagModel = item.Value;

    //      var col = contentBag.ColumnPosition;
    //      var row = contentBag.RowPosition;

    //      var position = TPosition.Create (col, row);

    //      var bagModelItem = new Shared.Layout.Bag.TModelItem (bagModel.BagInfo, bagModel.BagLayout, bagModel.BagImage);

    //      var modelInfo = new TModelInfo (TContentType.Bag);
    //      modelInfo.Insert (bagModelItem);

    //      if (Select (position) is TDashBoardItem dashItem) {
    //        dashItem.Update (modelInfo);

    //        ChangeStatus (position, bagModel.BagLayout.BagStyle, TDashBoardItem.TDashBoardStatus.Busy, TContentType.Bag);
    //        res = true; // must notify report

    //        // update content info collection
    //        var contentInfo = TContentInfo.CreateDefault;
    //        contentInfo.Select (bagModel.BagInfo.BagId, TContentType.Bag, position);

    //        contentInfoCollection.Add (contentInfo);
    //      }
    //    }
    //  }

    //  return (res);
    //}

    //internal void Select (Server.Models.Module.Slide.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  SlideSourceCollection.Clear ();

    //  foreach (var model in action.CollectionAction.ModelCollection) {
    //    var id = model.Value.SlideInfo.SlideId;

    //    if (IsRemovedItem (id)) {
    //      continue;
    //    }

    //    if (IsContentItem (id)) {
    //      continue;
    //    }

    //    var slideModel = new Shared.Module.Slide.TModelItem (model.Value.SlideInfo, model.Value.SlideLayout);

    //    var info = new TModelInfo (TContentType.Slide);
    //    info.Insert (slideModel);

    //    SlideSourceCollection.Add (info);
    //  }

    //  m_ContentSlideItems.Clear (); // no need anymore
    //}

    //internal bool SelectMany (Server.Models.Module.Slide.TEntityAction action, Collection<TContentInfo> contentInfoCollection)
    //{
    //  // action.CollectionAction.ModelCollection[SlideId] {Slide model}
    //  // action.CollectionAction.EntityCollection[SlideId].CollectionAction.ModelColletion[FrameId] {Frame model}

    //  var res = false;

    //  action.ThrowNull ();
    //  contentInfoCollection.ThrowNull ();

    //  foreach (var item in action.CollectionAction.ModelCollection) {
    //    var slideList = m_ContentSlides
    //      .Where (p => p.ChildId.Equals (item.Key))
    //      .ToList ()
    //    ;

    //    if (slideList.Count.Equals (1)) {
    //      var contentSlide = slideList [0];
    //      var slideModel = item.Value;

    //      var col = contentSlide.ColumnPosition;
    //      var row = contentSlide.RowPosition;

    //      var position = TPosition.Create (col, row);

    //      var slideModelItem = new Shared.Module.Slide.TModelItem (slideModel.SlideInfo, slideModel.SlideLayout);

    //      var modelInfo = new TModelInfo (TContentType.Slide);
    //      modelInfo.Insert (slideModelItem);

    //      if (Select (position) is TDashBoardItem dashItem) {
    //        dashItem.Update (modelInfo);

    //        ChangeStatus (position, slideModel.SlideLayout.SlideStyle, TDashBoardItem.TDashBoardStatus.Busy, TContentType.Slide);
    //        res = true; // must notify report

    //        // update content info collection
    //        var contentInfo = TContentInfo.CreateDefault;
    //        contentInfo.Select (slideModel.SlideInfo.SlideId, TContentType.Slide, position);

    //        contentInfoCollection.Add (contentInfo);
    //      }
    //    }
    //  }

    //  return (res);
    //}

    //internal void SelectModel (Server.Models.Module.Shelf.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  InfoModel.CopyFrom (action.ModelAction.ShelfInfo);
    //  LayoutModel.CopyFrom (action.ModelAction.ShelfLayout);
    //}
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

    //internal bool RequestContent (Server.Models.Infrastructure.TContextType.Context contextType, IList<Guid> idList)
    //{
    //  idList.ThrowNull ();

    //  switch (contextType) {
    //    case Server.Models.Infrastructure.TContextType.Context.Bag: {
    //        foreach (var item in m_ContentBags) {
    //          idList.Add (item.ChildId);
    //        }

    //        return (m_ContentBags.Count > 0);
    //      }

    //    case Server.Models.Infrastructure.TContextType.Context.Slide: {
    //        foreach (var item in m_ContentSlides) {
    //          idList.Add (item.ChildId);
    //        }

    //        return (m_ContentSlides.Count > 0);
    //      }
    //  }

    //  return (false);
    //}



    //internal void RequestContent (Server.Models.Module.Slide.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  action.IdCollection.Clear ();

    //  foreach (var item in m_ContentSlideItems) {
    //    action.IdCollection.Add (item);
    //  }
    //}

    //internal void RequestModel (Server.Models.Content.TEntityAction action)
    //{
    //  action.ThrowNull ();

    //  action.Id = action.ModelAction.ContentLayout.ContentId;

    //  foreach (var item in DashBoardItemSource) {
    //    if (item.IsBusy) {
    //      if (item.ContextId.NotEmpty ()) {
    //        switch (item.ContentType) {
    //          case TContentType.Bag: {
    //              Server.Models.Content.ContentLayout child = new Server.Models.Content.ContentLayout (
    //                item.ContextId,
    //                Server.Models.Infrastructure.TContextType.Context.Bag,
    //                item.ItemPosition.Column,
    //                item.ItemPosition.Row
    //              );

    //              child.CopyFromContent (action.ModelAction.ContentLayout);

    //              action.CollectionAction.ContentLayoutCollection.Add (child);
    //            }
    //            break;

    //          case TContentType.Slide: {
    //              Server.Models.Content.ContentLayout child = new Server.Models.Content.ContentLayout (
    //                item.ContextId,
    //                Server.Models.Infrastructure.TContextType.Context.Slide,
    //                item.ItemPosition.Column,
    //                item.ItemPosition.Row
    //              );

    //              child.CopyFromContent (action.ModelAction.ContentLayout);

    //              action.CollectionAction.ContentLayoutCollection.Add (child);
    //            }
    //            break;
    //        }
    //      }
    //    }
    //  }
    //}

    //internal void RequestReport (TReportData report)
    //{
    //  if (IsDashBoardEmpty ()) {
    //    report.SelectUnlock ();
    //  }

    //  if (IsDashBoardBusy ()) {
    //    report.SelectLock ();
    //  }
    //} 
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
    Collection<ExtensionNode>                                             m_NodeCollection;
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
