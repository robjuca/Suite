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
  public sealed class TComponentOperation
  {
    #region Data
    public enum TInternalOperation
    {
      None,
      Category,
      Id,
    };
    #endregion

    #region Property
    public Collection<int> CategoryCollection
    {
      get;
      private set;
    }

    public Collection<Guid> IdCollection
    {
      get;
      private set;
    }


    public Dictionary<Guid, ComponentRelation> ChildIdCollection
    {
      get;
      private set;
    }

    public Dictionary<Guid, Collection<ComponentRelation>> ParentIdCollection
    {
      get;
      private set;
    }

    public Dictionary<int, Collection<ComponentRelation>> ChildCategoryCollection
    {
      get;
      private set;
    }

    public Dictionary<int, Collection<ComponentRelation>> ParentCategoryCollection
    {
      get;
      private set;
    }

    public TInternalOperation Operation
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    TComponentOperation ()
    {
      CategoryCollection = new Collection<int> ();
      IdCollection = new Collection<Guid> ();

      ChildIdCollection = new Dictionary<Guid, ComponentRelation> ();
      ParentIdCollection = new Dictionary<Guid, Collection<ComponentRelation>> ();

      ChildCategoryCollection = new Dictionary<int, Collection<ComponentRelation>> ();
      ParentCategoryCollection = new Dictionary<int, Collection<ComponentRelation>> ();

      Operation = TInternalOperation.None;
    }

    TComponentOperation (TInternalOperation componentOperation)
      : this ()
    {
      Operation = componentOperation;
    }
    #endregion

    #region Members
    public bool IsComponentOperation (TInternalOperation componentOperation)
    {
      return (Operation.Equals (componentOperation));
    }

    public void SelectChild (Guid id, IList<ComponentRelation> list)
    {
      foreach (var ComponentRelation in list) {
        ChildIdCollection.Add (id, ComponentRelation);
      }
    }

    public void SelectParent (Guid id, IList<ComponentRelation> list)
    {
      ParentIdCollection.Add (id, new Collection<ComponentRelation> (list));
    }

    public void SelectChild (int typeItem, IList<ComponentRelation> list)
    {
      ChildCategoryCollection.Add (typeItem, new Collection<ComponentRelation> (list));
    }

    public void SelectParent (int typeItem, IList<ComponentRelation> list)
    {
      ParentCategoryCollection.Add (typeItem, new Collection<ComponentRelation> (list));
    }

    public void SelectByCategory (int typeItem)
    {
      CategoryCollection.Add (typeItem);
    }

    public void SelectById (Guid id)
    {
      IdCollection.Add (id);
    }

    public IList<ComponentRelation> RequestChildCategoryCollection ()
    {
      if (IsComponentOperation (TInternalOperation.Category)) {
        // First Type Only
        if (CategoryCollection.Count.Equals (1)) {
          if (ChildCategoryCollection.Count > 0) {
            return (ChildCategoryCollection [CategoryCollection [0]]);
          }
        }
      }

      return (null);
    }

    public IList<ComponentRelation> RequestParentCategoryCollection ()
    {
      if (IsComponentOperation (TInternalOperation.Category)) {
        // First Type Only
        if (CategoryCollection.Count.Equals (1)) {
          if (ParentCategoryCollection.Count > 0) {
            return (ParentCategoryCollection [CategoryCollection [0]]);
          }
        }
      }

      return (null);
    }

    public void Clear ()
    {
      ChildIdCollection.Clear ();
      ParentIdCollection.Clear ();

      ChildCategoryCollection.Clear ();
      ParentCategoryCollection.Clear ();
    }
    #endregion

    #region Property
    public static TComponentOperation Create (TInternalOperation contentOperation) => (new TComponentOperation (contentOperation));

    public static TComponentOperation CreateDefault => (new TComponentOperation ());
    #endregion
  };
  //---------------------------//

}  // namespace