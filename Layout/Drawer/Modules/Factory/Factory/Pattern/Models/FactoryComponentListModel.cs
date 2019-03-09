/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Server.Models.Component;

using Shared.ViewModel;
//---------------------------//

namespace Module.Factory.Pattern.Models
{
  public class TFactoryComponentListModel
  {
    #region Property
    public ObservableCollection<TComponentItemInfo> ComponentSourceCollection
    {
      get;
      private set;
    }

    public int ComponentCount
    {
      get
      {
        return (ComponentSourceCollection.Count);
      }
    }
    #endregion

    #region Constructor
    public TFactoryComponentListModel ()
    {
      ComponentSourceCollection = new ObservableCollection<TComponentItemInfo> ();

      m_ComponentRemovedItems = new Dictionary<Guid, TComponentItemInfo> ();
      m_ComponentTryToInsertItems = new Dictionary<Guid, TComponentItemInfo> ();

      m_ComponentItems = new Collection<Guid> ();

      m_NodeCollection = new Collection<ExtensionNode> ();
    }
    #endregion

    #region Members
    #region Select
    internal void SelectComponentRelation (TEntityAction action)
    {
      action.ThrowNull ();

      // category
      if (action.CollectionAction.IsComponentOperation (TComponentOperation.TInternalOperation.Category)) {
        int childCategory = Server.Models.Infrastructure.TCategoryType.ToValue (Server.Models.Infrastructure.TCategory.Shelf);

        // parent
        foreach (var item in action.ComponentOperation.ParentCategoryCollection) {
          foreach (var model in item.Value) {
            if (model.ChildCategory.Equals (childCategory)) {
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

      ComponentSourceCollection.Clear ();

      foreach (var modelItem in action.CollectionAction.ModelCollection) {
        var id = modelItem.Key;
        var modelAction = modelItem.Value;

        IList<ExtensionNode> list = m_NodeCollection
          .Where (p => p.ParentId.Equals (id))
          .ToList ()
        ;

        var componentModel = TComponentModel.Create (modelAction);
        var componentModelItem = TComponentModelItem.Create (componentModel);
        componentModelItem.Select (list);
        componentModelItem.Select (action.CategoryType.Category);

        var itemInfo = TComponentItemInfo.Create (componentModelItem);

        if (m_ComponentRemovedItems.ContainsKey (itemInfo.Id).IsFalse ()) {
          ComponentSourceCollection.Add (itemInfo);
        }
      }

      // try to insert
      foreach (var component in m_ComponentTryToInsertItems) {
        var itemInfo = component.Value;
        ComponentSourceCollection.Add (itemInfo);
      }

      SortCollection ();
    }

    internal void SelectDefault ()
    {
      m_ComponentItems.Clear ();
      m_ComponentRemovedItems.Clear ();
      m_ComponentTryToInsertItems.Clear ();
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

    //      var bagModelItem = new Shared.Module.Bag.TModelItem (bagModel.BagInfo, bagModel.BagLayout, bagModel.BagImage);

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
    internal void Drop (Guid id)
    {
      foreach (var item in ComponentSourceCollection) {
        if (item.Id.Equals (id)) {
          m_ComponentRemovedItems.Add (id, item);
          break;
        }
      }

      var listSource = ComponentSourceCollection
        .Where (p => p.Id.Equals (id))
        .ToList ()
      ;

      if (listSource.Count.Equals (1)) {
        ComponentSourceCollection.Remove (listSource [0]);
      }

      var list = m_ComponentTryToInsertItems
        .Where (p => p.Key.Equals (id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        m_ComponentTryToInsertItems.Remove (list [0].Key);
      }
    }

    internal bool Restore (Guid id)
    {
      Guid idToRemove = Guid.Empty;
      string style=string.Empty;

      foreach (var item in m_ComponentRemovedItems) {
        if (item.Key.Equals (id)) {
          var model = item.Value;

          idToRemove = model.Id;
          style = model.Style;

          break;
        }
      }

      if (idToRemove.NotEmpty ()) {
        ComponentSourceCollection.Add (m_ComponentRemovedItems [id]); // restore
        m_ComponentRemovedItems.Remove (idToRemove);

        SortCollection ();

        return (true);
      }

      return (false);
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
      componentModelItem.Select (action.CategoryType.Category);

      // nodes
      foreach (var item in action.CollectionAction.EntityCollection) {
        componentModelItem.Select (item.Value.CollectionAction.ExtensionNodeCollection);
      }

      var itemInfo = TComponentItemInfo.Create (componentModelItem);

      m_ComponentTryToInsertItems.Add (itemInfo.Id, itemInfo);

      // try to insert
      ComponentSourceCollection.Add (itemInfo);
      SortCollection ();

      return (itemInfo.Id);
    }
    #endregion
    #endregion

    #region Fields
    readonly Dictionary<Guid, TComponentItemInfo>                         m_ComponentRemovedItems;
    readonly Dictionary<Guid, TComponentItemInfo>                         m_ComponentTryToInsertItems;
    readonly Collection<Guid>                                             m_ComponentItems;
    Collection<ExtensionNode>                                             m_NodeCollection;
    #endregion

    #region Support
    bool IsRemovedItem (Guid id)
    {
      foreach (var item in m_ComponentRemovedItems) {
        if (item.Key.Equals (id)) {
          return (true);
        }
      }

      return (false);
    }

    bool IsContentItem (Guid id)
    {
      foreach (var item in m_ComponentItems) {
        if (item.Equals (id)) {
          return (true);
        }
      }

      return (false);
    }

    void SortCollection ()
    {
      var list = new List<TComponentItemInfo> (ComponentSourceCollection);

      ComponentSourceCollection.Clear ();
      ComponentSourceCollection = new ObservableCollection<TComponentItemInfo> (list.OrderBy (p => p.Name).ToList ());
    }
    #endregion
  };
  //---------------------------//

}  // namespace
