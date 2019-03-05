/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Helper;
using rr.Library.Message;
using rr.Library.Types;

using Shared.Resources;
using Shared.Types;
//---------------------------//

namespace Shared.Message
{
  //----- TShellMessage
  public class TShellMessage : TMessageModule
  {
    #region Constructor
    public TShellMessage (TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Shell, messageAction, TNode.CreateDefault, typeInfo)
    {
    }
    #endregion
  };
  //---------------------------//

  //----- TCollectionMessage
  public class TCollectionMessage : TMessageModule
  {
    #region Constructor
    public TCollectionMessage (TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Collection, messageAction, TNode.CreateDefault, typeInfo)
    {
    }

    public TCollectionMessage (TValidationResult result, TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Collection, messageAction, TNode.CreateDefault, typeInfo)
    {
      CopyResult (result);
    }
    #endregion
  };
  //---------------------------//

  //----- TFactoryMessage
  public class TFactoryMessage : TMessageModule
  {
    #region Constructor
    public TFactoryMessage (TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
    }

    public TFactoryMessage (TValidationResult result, TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
      CopyResult (result);
    }
    #endregion
  };
  //---------------------------//

  //----- TServicesMessage
  public class TServicesMessage : TMessageModule
  {
    #region Constructor
    public TServicesMessage (TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Services, messageAction, TNode.CreateDefault, typeInfo)
    {
    }

    public TServicesMessage (TValidationResult result, TMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Services, messageAction, TNode.CreateDefault, typeInfo)
    {
      CopyResult (result);
    }
    #endregion
  };
  //---------------------------//

  //----- TMessageModule
  public class TMessageModule : TMessage<TResource.TModule, TMessageAction, TSupport<TModuleArgument<TArgumentModule>>, TNode>
  {
    #region Constructor
    public TMessageModule (TResource.TModule moduleName, TMessageAction messageAction, TNode node, TTypeInfo typeInfo)
      : base (moduleName, messageAction, TDefault.SupportModule, node, typeInfo)
    {
    }
    #endregion
  };
  //---------------------------//

}  // namespace