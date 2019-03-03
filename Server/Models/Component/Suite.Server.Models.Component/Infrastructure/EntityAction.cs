/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Linq;

using Server.Models.Infrastructure;
//---------------------------//

namespace Server.Models.Component
{
  public sealed class TEntityAction : TEntityAction<TModelAction, TCollectionAction>
  {
    #region Property
    public TComponentOperation ComponentOperation
    {
      get
      {
        return (CollectionAction.ComponentOperation);
      }
    }

    public TComponentModel ComponentModel
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TEntityAction (TModelAction model, TCollectionAction collection, TCategory category, string connectionString)
      : base (model, collection, category, connectionString)
    {
      ComponentModel = TComponentModel.CreateModel;
    }

    public TEntityAction (TCategory category, string connectionString)
      : base (TModelAction.CreateDefault, TCollectionAction.CreateDefault, category, connectionString)
    {
      ComponentModel = TComponentModel.CreateModel;
    }

    public TEntityAction (TCategory category, string connectionString, object param1, object param2 = null)
      : base (TModelAction.CreateDefault, TCollectionAction.CreateDefault, category, connectionString, param1, param2)
    {
      ComponentModel = TComponentModel.CreateModel;
    }

    TEntityAction ()
      : base (TModelAction.CreateDefault, TCollectionAction.CreateDefault, TCategory.None, string.Empty)
    {
      ComponentModel = TComponentModel.CreateModel;
    }

    TEntityAction (TCategory category)
      : base (TModelAction.CreateDefault, TCollectionAction.CreateDefault, category, string.Empty)
    {
      ComponentModel = TComponentModel.CreateModel;
    }
    #endregion

    #region Members
    public void SelectModel (TComponentModel componentModel)
    {
      if (componentModel.NotNull ()) {
        ComponentModel = componentModel;

        Id = ComponentModel.Id;

        ModelAction.SettingsModel.CopyFrom (ComponentModel.SettingsModel);

        ModelAction.ComponentInfoModel.CopyFrom (ComponentModel.InfoModel);
        ModelAction.ComponentStatusModel.CopyFrom (ComponentModel.StatusModel);

        ModelAction.ExtensionDocumentModel.CopyFrom (ComponentModel.DocumentModel);
        ModelAction.ExtensionImageModel.CopyFrom (ComponentModel.ImageModel);
        ModelAction.ExtensionGeometryModel.CopyFrom (ComponentModel.GeometryModel);
        ModelAction.ExtensionLayoutModel.CopyFrom (ComponentModel.LayoutModel);
        ModelAction.ExtensionTextModel.CopyFrom (ComponentModel.TextModel);
        ModelAction.ExtensionNodeModel.CopyFrom (ComponentModel.NodeModel);

        CollectionAction.SetCollection (ComponentModel.NodeModelCollection);
      }
    }

    public void SelectMany (IList<Guid> idList)
    {
      if (idList.NotNull ()) {
        CollectionAction.ModelCollection.Clear ();
       
        foreach (var id in idList) {
          CollectionAction.ModelCollection.Add (id, TModelAction.CreateDefault);
        }
      }
    }

    public void RequestExtension (TComponentExtension componentExtension)
    {
      if (componentExtension.NotNull ()) {
        // request CategoryRelation (Extension)
        var categoryValue = TCategoryType.ToValue (CategoryType.Category);

        var categoryRelationList = CollectionAction.CategoryRelationCollection
          .Where (p => p.Category.Equals (categoryValue))
          .ToList ()
        ;

        if (categoryRelationList.Count.Equals (1)) {
          var categoryRelation = categoryRelationList [0]; // get extension using TComponentExtension

          var extension = TComponentExtension.Create (categoryRelation.Extension);
          extension.Request ();

          componentExtension.CopyFrom (extension);
        }
      }
    }

    public static TEntityAction Request (IEntityAction action)
    {
      return (action as TEntityAction);
    }

    public static TEntityAction Request (IEntityAction action, string databaseConnectionString)
    {
      (action as TEntityAction).SelectConnection (databaseConnectionString);

      return (action as TEntityAction);
    }

    public static TEntityAction Request (IEntityAction action, string databaseConnectionString, object param1)
    {
      (action as TEntityAction).SelectConnection (databaseConnectionString);
      (action as TEntityAction).Param1 = param1;

      return (action as TEntityAction);
    }

    public static TEntityAction Create (TCategory category, TOperation operation)
    {
      var model = Create (category);
      model.Operation.Select (category, operation);

      return (model);
    }

    public static TEntityAction Create (TCategory category, TOperation operation, Infrastructure.TExtension extension)
    {
      var model = Create (category);
      model.Operation.Select (category, operation, extension);

      return (model);
    }
    #endregion

    #region Static
    public static TEntityAction Create (TCategory category) => new TEntityAction (category);

    public static TEntityAction CreateDefault => new TEntityAction (); 
    #endregion
  }
  //---------------------------//

}  // namespace