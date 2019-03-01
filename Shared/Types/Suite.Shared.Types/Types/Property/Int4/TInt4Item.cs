/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public class TInt4Item
  {
    #region Property
    public string Int4String
    {
      get;
    }

    public int Int4Value
    {
      get;
    }
    #endregion

    #region Constructor
    public TInt4Item (string int4String, int int4Value)
    {
      Int4String = int4String;
      Int4Value = int4Value;
    }

    public TInt4Item (TInt4Item alias)
    {
      if (alias.NotNull ()) {
        Int4String = alias.Int4String;
        Int4Value = alias.Int4Value;
      }
    }

    TInt4Item ()
    {
      Int4String = string.Empty;
      Int4Value = 0;
    }
    #endregion

    #region Members
    public bool Contains (TInt4Item alias)
    {
      if (alias.NotNull ()) {
        return (Int4Value.Equals (alias.Int4Value));
      }

      return (false);
    }

    public bool Contains (int int4Value)
    {
      return (Int4Value.Equals (int4Value));
    }
    #endregion

    #region Static
    public static TInt4Item CreateDefault => new TInt4Item (); 
    #endregion
  };
  //---------------------------//

}  // namespace