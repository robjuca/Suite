/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryListModel : NotificationObject
  {
    #region Property
    public Collection<TComponentSelectorInfo> ComponentSelectorSource
    {
      get;
      private set;
    }
    
    public ObservableCollection<TComponentSourceInfo> ComponentModelCollection
    {
      get;
      private set;
    }

    public int ComponentSelectorIndex
    {
      get;
      set;
    }

    public string Style
    {
      get;
      set;
    }

    public int ComponentCount
    {
      get
      {
        return (ComponentModelCollection.Count);
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

    public bool IsEnabledView
    {
      get;
      set;
    }

    public bool IsCategoryEnabled
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryListModel ()
    {
      ComponentSelectorSource = new Collection<TComponentSelectorInfo>
      {
        new TComponentSelectorInfo (Server.Models.Infrastructure.TCategory.None),
        new TComponentSelectorInfo (Server.Models.Infrastructure.TCategory.Document),
        new TComponentSelectorInfo (Server.Models.Infrastructure.TCategory.Image)
      };

      ComponentSelectorIndex = 0;

      ComponentModelCollection = new ObservableCollection<TComponentSourceInfo> ();

      IsEnabledView = true;
      IsCategoryEnabled = true;

      m_ComponentModelItems = new Collection<TComponentModelItem> ();

      m_HorizontalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      m_HorizontalStyleInfo.Select (TContentStyle.Style.mini);

      m_VerticalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Vertical);
      m_VerticalStyleInfo.Select (TContentStyle.Style.mini);
    }
    #endregion

    #region Members
    internal void HorizontalStyleChanged (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      m_HorizontalStyleInfo.Select (action.ModelAction.ExtensionLayoutModel.StyleHorizontal);
      SelectStyle ();
    }

    internal void VerticalStyleChanged (Server.Models.Component.TEntityAction action)
    {
      action.ThrowNull ();

      m_VerticalStyleInfo.Select (action.ModelAction.ExtensionLayoutModel.StyleVertical);
      SelectStyle ();
    }

    internal void SelectModel (Server.Models.Component.TEntityAction action)
    {
      // action.ModelAction {bag model}
      // action.CollectionAction.ExtensionNodeCollection {node info}
      // action.CollectionAction.ModelCollection [id, TModelAction] {bag child} only one child (Document or Image or Video)

      action.ThrowNull ();

      m_ComponentModelItems.Clear ();
      IsCategoryEnabled = true;

      int index = 0;

      if (action.CollectionAction.ModelCollection.Count.Equals (1)) {
        //var modelAction = action.CollectionAction.ModelCollection [0];
      }

      foreach (var item in action.CollectionAction.ModelCollection) {
        var id = item.Key;
        var modelAction = item.Value;

        var list = action.CollectionAction.ExtensionNodeCollection
          .Where (p => p.ChildId.Equals (id))
          .ToList ()
        ;

        if (list.Count.Equals (1)) {
          var node = list [0];
          var category = Server.Models.Infrastructure.TCategoryType.FromValue (node.ChildCategory);
          var componentModel = Server.Models.Component.TComponentModel.Create (modelAction);
          var model = TComponentModelItem.Create (componentModel);

          m_ComponentModelItems.Add (model);

          if (IsCategoryEnabled) {
            switch (category) {
              case Server.Models.Infrastructure.TCategory.Document: {
                  index = 1;
                }
                break;

              case Server.Models.Infrastructure.TCategory.Image: {
                  index = 2;
                }
                break;
            }
          }
        }
      }

      ComponentSelectorIndex = index;

      IsCategoryEnabled = ComponentSelectorIndex.Equals (0);
    }

    internal void RequestSelectedComponents (IList<TComponentSourceInfo> list)
    {
      list.ThrowNull ();

      foreach (var info in ComponentModelCollection) {
        switch (Selector.Category) {
          case Server.Models.Infrastructure.TCategory.Document: {
              if (info.DocumentChecked) {
                list.Add (info);
              }
            }
            break;

          case Server.Models.Infrastructure.TCategory.Image: {
              if (info.ImageChecked) {
                list.Add (info);
              }
            }
            break;
        }
      }
    }

    internal Server.Models.Infrastructure.TCategory RequestComponentSelector ()
    {
      if (Selector.Category.Equals (Server.Models.Infrastructure.TCategory.None)) {
        ComponentModelCollection.Clear ();
      }

      DocumentVisibility = Selector.Category.Equals (Server.Models.Infrastructure.TCategory.Document) ? Visibility.Visible : Visibility.Collapsed;
      ImageVisibility = Selector.Category.Equals (Server.Models.Infrastructure.TCategory.Image) ? Visibility.Visible : Visibility.Collapsed;

      return (ComponentSelectorSource [ComponentSelectorIndex].Category);
    }

    internal void SelectComponentSelector (Server.Models.Component.TEntityAction action)
    {
      // action.CollectionAction.ModelCollection[id] {model}

      action.ThrowNull ();

      ComponentModelCollection.Clear ();

      if (Selector.Category.Equals (Server.Models.Infrastructure.TCategory.None).IsFalse ()) {
        var category = Selector.Category;

        foreach (var item in action.CollectionAction.ModelCollection) {
          var id = item.Key;
          var modelAction = item.Value;

          // same style only
          if (IsSameStyle (modelAction)) {
            // enabled only 
            if (modelAction.ComponentInfoModel.Enabled) {
              // can not be part of node
              if (modelAction.ExtensionNodeModel.ChildId.Equals (id).IsFalse ()) {
                var model = Server.Models.Component.TComponentModel.Create (modelAction);

                ComponentModelCollection.Add (new TComponentSourceInfo (category, TComponentModelItem.Create (model)));
              }
            }
          }
        }
      }

      if (m_ComponentModelItems.Count.Equals(0).IsFalse ()) {
        foreach (var modelItem in m_ComponentModelItems) {
          var info = new TComponentSourceInfo (Selector.Category, modelItem);
          info.ValidateCheck ();

          ComponentModelCollection.Add (info);
        }

        m_ComponentModelItems.Clear ();

        RaisePropertyChanged ("CategoryChanged");
      }
    }

    internal void ComponentSelected (TComponentSourceInfo info)
    {
      var list = ComponentModelCollection
        .Where (p => p.Id.Equals (info.Id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        list [0].ValidateCheck ();
      }
    }

    internal void CanRemove (TComponentSourceInfo info)
    {
      var list = ComponentModelCollection
        .Where (p => p.Id.Equals (info.Id))
        .ToList ()
      ;

      if (list.Count.Equals (1)) {
        list [0].CanRemove ();
      }
    }

    internal void LockEnter ()
    {
      IsEnabledView = false;
    }

    internal void LockLeave ()
    {
      IsEnabledView = true;
    }

    internal void Cleanup ()
    {
      ImageVisibility = Visibility.Collapsed;
      DocumentVisibility = Visibility.Collapsed;

      ComponentModelCollection.Clear ();
      ComponentSelectorIndex = 0;

      IsEnabledView = true;
      IsCategoryEnabled = true;
    }
    #endregion

    #region Property
    TComponentSelectorInfo Selector
    {
      get
      {
        return (ComponentSelectorSource [ComponentSelectorIndex]);
      }
    }
    #endregion

    #region Fields
    readonly TStyleInfo                                                   m_HorizontalStyleInfo;
    readonly TStyleInfo                                                   m_VerticalStyleInfo;
    readonly Collection<TComponentModelItem>                              m_ComponentModelItems;
    #endregion

    #region Support
    void SelectStyle ()
    {
      Style = $"[ style: {m_HorizontalStyleInfo.StyleFullString}, {m_VerticalStyleInfo.StyleFullString}]";

      ComponentSelectorIndex = 0;
    }

    bool IsSameStyle (Server.Models.Component.TModelAction modelAction)
    {
      var hs = TContentStyle.TryToParse (modelAction.ExtensionLayoutModel.StyleHorizontal);
      var vs = TContentStyle.TryToParse (modelAction.ExtensionLayoutModel.StyleVertical);

      return (m_HorizontalStyleInfo.Style.Equals (hs) && m_VerticalStyleInfo.Style.Equals (vs));
    }
    #endregion
  };
  //---------------------------//

  //----- TComponentSelectorInfo
  public class TComponentSelectorInfo
  {
    #region Property
    public string Name
    {
      get
      {
        return (Category.ToString ());
      }
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
    }
    #endregion

    #region Constructor
    public TComponentSelectorInfo (Server.Models.Infrastructure.TCategory category)
    {
      Category = category;
    }
    #endregion
  };
  //---------------------------//

  //----- TComponentSourceInfo
  public class TComponentSourceInfo
  {
    #region Property
    public string Name
    {
      get
      {
        return (Model.Name);
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

    public Visibility CanRemoveVisibility
    {
      get;
      set;
    }

    public bool ImageChecked
    {
      get;
      set;
    }

    public bool DocumentChecked
    {
      get;
      set;
    }

    public TComponentModelItem Model
    {
      get;
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
    }

    public Guid Id
    {
      get
      {
        return (Model.Id);
      }
    }
    #endregion

    #region Constructor
    public TComponentSourceInfo (Server.Models.Infrastructure.TCategory category, TComponentModelItem item)
    {
      Category = category;

      Model = TComponentModelItem.CreateDefault;
      Model.CopyFrom (item);

      DocumentVisibility = Category.Equals (Server.Models.Infrastructure.TCategory.Document) ? Visibility.Visible : Visibility.Collapsed;
      ImageVisibility = Category.Equals (Server.Models.Infrastructure.TCategory.Image) ? Visibility.Visible : Visibility.Collapsed;
      CanRemoveVisibility = Visibility.Collapsed;

      ImageChecked = false;
      DocumentChecked = false;
    }
    #endregion

    #region Members
    internal void ValidateCheck ()
    {
      ImageChecked = false;
      DocumentChecked = false;

      switch (Category) {
        case Server.Models.Infrastructure.TCategory.Document:
          DocumentChecked = true;
          CanRemoveVisibility = Visibility.Visible;
          break;

        case Server.Models.Infrastructure.TCategory.Image:
          ImageChecked = true;
          break;
      }
    }

    internal void CanRemove ()
    {
      if (Category.Equals (Server.Models.Infrastructure.TCategory.Document)) {
        DocumentChecked = false;
        CanRemoveVisibility = Visibility.Collapsed;
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace
