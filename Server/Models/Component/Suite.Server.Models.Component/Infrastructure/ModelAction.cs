/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Component
{
  public sealed class TModelAction
  {
    #region Property
    #region Settings
    public Settings SettingsModel
    {
      get;
      private set;
    }
    #endregion

    #region Category
    public CategoryRelation CategoryRelationModel
    {
      get;
      private set;
    } 
    #endregion

    #region Component
    public ComponentDescriptor ComponentDescriptorModel
    {
      get;
      private set;
    }

    public ComponentInfo ComponentInfoModel
    {
      get;
      private set;
    }

    public ComponentStatus ComponentStatusModel
    {
      get;
      private set;
    }

    public ComponentRelation ComponentRelationModel
    {
      get;
      private set;
    }
    #endregion

    #region Extension
    public ExtensionDocument ExtensionDocumentModel
    {
      get;
      private set;
    }

    public ExtensionGeometry ExtensionGeometryModel
    {
      get;
      private set;
    }

    public ExtensionImage ExtensionImageModel
    {
      get;
      private set;
    }

    public ExtensionLayout ExtensionLayoutModel
    {
      get;
      private set;
    }

    public ExtensionNode ExtensionNodeModel
    {
      get;
      private set;
    }

    public ExtensionText ExtensionTextModel
    {
      get;
      private set;
    }
    #endregion
    #endregion

    #region Constructor
    TModelAction ()
    {
      // Settings
      SettingsModel = Settings.CreateDefault;

      // Category
      CategoryRelationModel = CategoryRelation.CreateDefault;

      // Component
      ComponentDescriptorModel = ComponentDescriptor.CreateDefault;
      ComponentInfoModel = ComponentInfo.CreateDefault;
      ComponentStatusModel = ComponentStatus.CreateDefault;
      ComponentRelationModel = ComponentRelation.CreateDefault;

      // Extension
      ExtensionDocumentModel = ExtensionDocument.CreateDefault;
      ExtensionGeometryModel = ExtensionGeometry.CreateDefault;
      ExtensionImageModel = ExtensionImage.CreateDefault;
      ExtensionLayoutModel = ExtensionLayout.CreateDefault;
      ExtensionNodeModel = ExtensionNode.CreateDefault;
      ExtensionTextModel = ExtensionText.CreateDefault;
    }
    #endregion

    #region Members
    public void CopyFrom (TModelAction alias)
    {
      if (alias.NotNull ()) {
        SettingsModel.CopyFrom (alias.SettingsModel);

        CategoryRelationModel.CopyFrom (alias.CategoryRelationModel);

        ComponentDescriptorModel.CopyFrom (alias.ComponentDescriptorModel);
        ComponentInfoModel.CopyFrom (alias.ComponentInfoModel);
        ComponentStatusModel.CopyFrom (alias.ComponentStatusModel);
        ComponentRelationModel.CopyFrom (alias.ComponentRelationModel);

        ExtensionDocumentModel.CopyFrom (alias.ExtensionDocumentModel);
        ExtensionGeometryModel.CopyFrom (alias.ExtensionGeometryModel);
        ExtensionImageModel.CopyFrom (alias.ExtensionImageModel);
        ExtensionLayoutModel.CopyFrom (alias.ExtensionLayoutModel);
        ExtensionNodeModel.CopyFrom (alias.ExtensionNodeModel);
        ExtensionTextModel.CopyFrom (alias.ExtensionTextModel);
      }
    }
    #endregion

    #region Static
    public static TModelAction CreateDefault => (new TModelAction ()); 
    #endregion
  };
  //---------------------------//

}  // namespace