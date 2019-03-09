/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using Shared.Module.Document;
//---------------------------//

namespace Module.Factory.Pattern.Models
{
  public class TFactoryDesignModel
  {
    #region Property
    public TComponentControlModel ComponentControlModel
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    public TFactoryDesignModel ()
    {
      ComponentControlModel = TComponentControlModel.CreateDefault;
    }
    #endregion

    #region Members
    internal void SelectModel (string propertyName, Server.Models.Infrastructure.IEntityAction entityAction)
    {
      ComponentControlModel.SelectModel (propertyName, Server.Models.Component.TEntityAction.Request (entityAction));
    }

    internal void Cleanup ()
    {
      ComponentControlModel.Cleanup ();
    }
    #endregion
  };
  //---------------------------//

}  // namespace
