/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using rr.Library.Helper;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TEntityActionBase<T> : IEntityAction
    where T : TCategoryType
  {
    #region Property
    public T CategoryType
    {
      get;
      private set;
    }

    public Collection<Guid> IdCollection
    {
      get;
      private set;
    }

    public TValidationResult Result
    {
      get;
      set;
    }

    public string ConnectionString
    {
      get;
      set;
    }

    public Guid Id
    {
      get;
      set;
    }

    public object Param1
    {
      get;
      set;
    }

    public object Param2
    {
      get;
      set;
    }

    public TEntityOperation<TCategoryType> Operation
    {
      get;
    }

    public TSummary Summary
    {
      get;
    }
    #endregion

    #region Constructor
    protected TEntityActionBase ()
    {
      IdCollection = new Collection<Guid> ();

      Result = TValidationResult.CreateDefault;
      Operation = TEntityOperation<TCategoryType>.Create (TCategoryType.Create (TCategory.None));

      Summary = TSummary.CreateDefault;
    }

    public TEntityActionBase (T categoryType, string connectionString)
      : this ()
    {
      CategoryType = categoryType;

      ConnectionString = connectionString;
      Operation.Select (categoryType);
    }

    public TEntityActionBase (T categoryType, string connectionString, object param1, object param2)
      : this ()
    {
      CategoryType = categoryType;

      ConnectionString = connectionString;
      Operation.Select (categoryType);

      Param1 = param1;
      Param2 = param2;
    }
    #endregion

    #region Members
    public void SelectConnection (string connectionString)
    {
      ConnectionString = connectionString;
    }

    public void CopyFrom (IList<Guid> alias)
    {
      if (alias.NotNull ()) {
        IdCollection = new Collection<Guid> (alias);
      }
    }
    #endregion
  };
  //---------------------------//

}  // namespace