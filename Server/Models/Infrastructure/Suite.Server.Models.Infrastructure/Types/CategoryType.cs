/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TCategoryType
  {
    #region Property
    public TCategory Category
    {
      get;
      private set;
    }

    public static string [] Names 
    {
      get
      {
        return (Enum.GetNames (typeof (TCategory)));
      }
    }
    #endregion

    #region Constructor
    TCategoryType (TCategory category)
    {
      Category = category;
    }
    #endregion

    #region Members
    public bool IsCategory (TCategory category)
    {
      return (Category.Equals (category));
    }

    public bool IsCategory (TCategoryType alias)
    {
      return (alias.IsNull () ? false : alias.Equals (alias.Category));
    }

    public void Select (TCategory category)
    {
      Category = category;
    }

    public void CopyFrom (TCategoryType alias)
    {
      if (alias.NotNull ()) {
        Category = alias.Category;
      }
    }

    public static int ToValue (TCategory category)
    {
      return ((int) category);
    }

    public static TCategory FromValue (int category)
    {
      return (Enum.IsDefined (typeof (TCategory), category)  ? (TCategory) category : TCategory.None);
    }
    #endregion

    #region Static
    public static TCategoryType Create (TCategory category) => new TCategoryType (category); 
    #endregion
  };
  //---------------------------//

}  // namespace