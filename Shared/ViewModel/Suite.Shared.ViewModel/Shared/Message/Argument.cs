/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Message;
using rr.Library.Types;

using Shared.Message;
//---------------------------//

namespace Shared.ViewModel
{
  //----- TDefault
  public static class TDefault
  {
    #region TSupport
    public static TSupport<TInternalArgument<TArgumentInternal>> SupportInternal
    {
      get
      {
        return (new TSupport<TInternalArgument<TArgumentInternal>> (ArgumentInternal));
      }
    }
    #endregion

    #region TArgument
    public static TInternalArgument<TArgumentInternal> ArgumentInternal
    {
      get
      {
        return (new TInternalArgument<TArgumentInternal> (new TArgumentInternal ()));
      }
    }
    #endregion
  };
  //---------------------------//

  //----- TArgumentInternal
  public class TArgumentInternal : TArgumentInternal<TComponentModelItem>
  {
    #region Property
    public TAuthentication Authentication
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TArgumentInternal ()
      : base (TComponentModelItem.CreateDefault)
    {
      Authentication = TAuthentication.None;
    }
    #endregion

    #region Members
    public void Select (TAuthentication authentication)
    {
      Authentication = authentication;
    }
    #endregion
  };
  //---------------------------//

}  // namespace