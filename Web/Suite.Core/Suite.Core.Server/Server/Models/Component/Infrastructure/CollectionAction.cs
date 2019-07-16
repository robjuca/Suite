/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//---------------------------//

namespace Server.Models.Component
{
  public sealed class TCollectionAction
  {
    #region Property
    #region Category
    public Collection<CategoryRelation> CategoryRelationCollection
    {
      get;
      private set;
    } 
    #endregion

    #region Component
    public Collection<ComponentDescriptor> ComponentDescriptorCollection
    {
      get;
      private set;
    }

    public Collection<ComponentInfo> ComponentInfoCollection
    {
      get;
      private set;
    }

    public Collection<ComponentStatus> ComponentStatusCollection
    {
      get;
      private set;
    }

    public Collection<ComponentRelation> ComponentRelationCollection
    {
      get;
      private set;
    }
    #endregion

    #region Extension
    public Collection<ExtensionDocument> ExtensionDocumentCollection
    {
      get;
      private set;
    }

    public Collection<ExtensionGeometry> ExtensionGeometryCollection
    {
      get;
      private set;
    }

    public Collection<ExtensionImage> ExtensionImageCollection
    {
      get;
      private set;
    }

    public Collection<ExtensionLayout> ExtensionLayoutCollection
    {
      get;
      private set;
    }

    public Collection<ExtensionNode> ExtensionNodeCollection
    {
      get;
      private set;
    }

    public Collection<ExtensionText> ExtensionTextCollection
    {
      get;
      private set;
    }
    #endregion

    #region ById
    public Dictionary<Guid, TModelAction> ModelCollection
    {
      get;
      private set;
    }

    public Dictionary<Guid, TEntityAction> EntityCollection
    {
      get;
      private set;
    }
    #endregion

    public TComponentOperation ComponentOperation
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TCollectionAction ()
    {
      // Category
      CategoryRelationCollection = new Collection<CategoryRelation> ();

      // Component
      ComponentDescriptorCollection = new Collection<ComponentDescriptor> ();
      ComponentInfoCollection = new Collection<ComponentInfo> ();
      ComponentStatusCollection = new Collection<ComponentStatus> ();
      ComponentRelationCollection = new Collection<ComponentRelation> ();

      // Extension
      ExtensionDocumentCollection = new Collection<ExtensionDocument> ();
      ExtensionGeometryCollection = new Collection<ExtensionGeometry> ();
      ExtensionImageCollection = new Collection<ExtensionImage> ();
      ExtensionLayoutCollection = new Collection<ExtensionLayout> ();
      ExtensionNodeCollection = new Collection<ExtensionNode> ();
      ExtensionTextCollection = new Collection<ExtensionText> ();

      // ById
      ModelCollection = new Dictionary<Guid, TModelAction> ();
      EntityCollection = new Dictionary<Guid, TEntityAction> ();

      ComponentOperation = TComponentOperation.CreateDefault;
    }
    #endregion

    #region Members
    public void SetCollection (IList<CategoryRelation> list)
    {
      CategoryRelationCollection = new Collection<CategoryRelation> (list);
    }

    public void SetCollection (IList<ComponentDescriptor> list)
    {
      ComponentDescriptorCollection = new Collection<ComponentDescriptor> (list);
    }

    public void SetCollection (IList<ComponentInfo> list)
    {
      ComponentInfoCollection = new Collection<ComponentInfo> (list);
    }

    public void SetCollection (IList<ComponentStatus> list)
    {
      ComponentStatusCollection = new Collection<ComponentStatus> (list);
    }

    public void SetCollection (IList<ComponentRelation> list)
    {
      ComponentRelationCollection = new Collection<ComponentRelation> (list);
    }

    public void SetCollection (IList<ExtensionDocument> list)
    {
      ExtensionDocumentCollection = new Collection<ExtensionDocument> (list);
    }

    public void SetCollection (IList<ExtensionGeometry> list)
    {
      ExtensionGeometryCollection = new Collection<ExtensionGeometry> (list);
    }

    public void SetCollection (IList<ExtensionImage> list)
    {
      ExtensionImageCollection = new Collection<ExtensionImage> (list);
    }

    public void SetCollection (IList<ExtensionLayout> list)
    {
      ExtensionLayoutCollection = new Collection<ExtensionLayout> (list);
    }

    public void SetCollection (IList<ExtensionNode> list)
    {
      ExtensionNodeCollection = new Collection<ExtensionNode> (list);
    }

    public void SetCollection (IList<ExtensionText> list)
    {
      ExtensionTextCollection = new Collection<ExtensionText> (list);
    }

    public bool IsComponentOperation (TComponentOperation.TInternalOperation componentOperation)
    {
      return (ComponentOperation.IsComponentOperation (componentOperation));
    }

    public void SelectComponentOperation (TComponentOperation.TInternalOperation operation)
    {
      ComponentOperation = TComponentOperation.Create (operation);
    }
    #endregion

    #region Static
    public static TCollectionAction CreateDefault => (new TCollectionAction ()); 
    #endregion
  }
  //---------------------------//

}  // namespace