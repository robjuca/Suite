/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Shared.ViewModel;

using Shared.Layout.Chest;
//---------------------------//

namespace Layout.Factory.Pattern.Models
{
  public class TFactoryDisplayModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }

    public TComponentModelItem ComponentModelItem
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TFactoryDisplayModel ()
    {
      Cleanup ();
    }
    #endregion

    #region Members
    internal void Cleanup ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault; // control model
      ComponentModelItem = TComponentModelItem.CreateDefault; //  Chest model
    }
    #endregion
  };
  //---------------------------//

}  // namespace