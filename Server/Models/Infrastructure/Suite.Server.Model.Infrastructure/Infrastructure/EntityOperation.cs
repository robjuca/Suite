/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TEntityOperation<T> 
    where T : TCategoryType
  {
    #region Property
    public TCategoryType CategoryType
    {
      get;
      private set;
    }

    public TOperation Operation
    {
      get;
      private set;
    }

    public TExtension Extension
    {
      get;
      private set;
    }

    public bool HasOperation
    {
      get
      {
        return (Operation.NotEquals (TOperation.None));
      }
    }

    public bool HasExtension
    {
      get
      {
        return (Extension.NotEquals (TExtension.None));
      }
    }
    #endregion

    #region Constructor
    TEntityOperation (T categoryType)
    {
      CategoryType = categoryType;

      Operation = TOperation.None;
      Extension = TExtension.None;
    }
    #endregion

    #region Members
    public void Select (TCategoryType categoryType)
    {
      CategoryType.CopyFrom (categoryType);
    }

    public void Select (TCategory category)
    {
      CategoryType.Select (category);
    }

    public void Select (TCategory category, TOperation operation)
    {
      CategoryType.Select (category);

      Operation = operation;
    }

    public void Select (TCategory category, TOperation operation, TExtension extension)
    {
      CategoryType.Select (category);

      Operation = operation;
      Extension = extension;
    }

    public bool IsCategory (TCategory category)
    {
      return (CategoryType.IsCategory (category));
    }

    public bool IsOperation (TOperation operation)
    {
      return (Operation.Equals (operation));
    }

    public bool IsExtension (TExtension extension)
    {
      return (Extension.Equals (extension));
    }

    public bool IsOperation (TOperation operation, TExtension extension)
    {
      return (Operation.Equals (operation) && Extension.Equals (extension));
    }
    #endregion

    #region Static 
    public static TEntityOperation<TCategoryType> Create (TCategoryType categoryType) => new TEntityOperation<TCategoryType> (categoryType); 
    #endregion
  };
  //---------------------------//

}  // namespace