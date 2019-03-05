/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Types;
//---------------------------//

namespace Shared.Types
{
  public class TRequestData
  {
    #region Property
    public TWhat What
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TRequestData (TWhat what)
      : this ()
    {
      What = what;
    }

    public TRequestData (TRequestData alias)
    {
      CopyFrom (alias);
    }

    protected TRequestData ()
    {
      Clean ();
    }
    #endregion

    #region Members
    public void Select (TWhat what)
    {
      What = what;
    }

    public bool IsWhat (TWhat what)
    {
      return (What.Equals (what));
    }

    public bool IsRequest (TWhat what)
    {
      return (What.Equals (what));
    }

    public void CopyFrom (TRequestData alias)
    {
      Clean ();

      if (alias != null) {
        What = alias.What;
      }
    }
    #endregion

    #region Property
    public static TRequestData CreateDefault => (new TRequestData ());
    #endregion

    #region Support
    void Clean ()
    {
      What = TWhat.None;
    } 
    #endregion
  };
  //---------------------------//

}  // namespace