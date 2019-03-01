/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
//---------------------------//

namespace Shared.Message
{
  public class TArgumentInternal<I> : TArgumentTypesDefault
  {
    #region Property
    public I Item
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TArgumentInternal (I item)
    {
      Item = item;
    }
    #endregion
  };
  //---------------------------//

}  // namespace