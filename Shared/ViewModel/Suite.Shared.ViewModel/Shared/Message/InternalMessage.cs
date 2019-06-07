/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Helper;
using rr.Library.Message;
using rr.Library.Types;

using Shared.Message;
using Shared.Resources;
using Shared.Types;
//---------------------------//

namespace Shared.ViewModel
{
  //----- TCollectionMessageInternal
  public class TCollectionMessageInternal : TMessageInternal
  {
    #region Constructor
    public TCollectionMessageInternal (TInternalMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Collection, messageAction, TNode.CreateDefault, typeInfo)
    {
    }

    public TCollectionMessageInternal (TInternalMessageAction messageAction, TChild child, TTypeInfo typeInfo)
      : base (TResource.TModule.Collection, messageAction, TNode.CreateDefault, typeInfo)
    {
      Node.SelectRelationChild (child);
    }

    public TCollectionMessageInternal (TValidationResult result, TInternalMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Collection, messageAction, TNode.CreateDefault, typeInfo)
    {
      CopyResult (result);
    }
    #endregion
  };
  //---------------------------//

  //----- TCollectionSiblingMessageInternal
  public class TCollectionSiblingMessageInternal : TMessageInternal
  {
    #region Constructor
    public TCollectionSiblingMessageInternal (TInternalMessageAction messageAction, TChild child, TTypeInfo typeInfo)
      : base (TResource.TModule.Collection, messageAction, TNode.CreateDefault, typeInfo)
    {
      Node.SelectRelationSibling (child);
    }
    #endregion
  };
  //---------------------------//

  //----- TFactoryMessageInternal
  public class TFactoryMessageInternal : TMessageInternal
  {
    #region Constructor
    public TFactoryMessageInternal (TInternalMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
    }

    public TFactoryMessageInternal (TInternalMessageAction messageAction, TChild child, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
      Node.SelectRelationChild (child);
    }

    public TFactoryMessageInternal (TValidationResult result, TInternalMessageAction messageAction, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
      CopyResult (result);
    }

    public TFactoryMessageInternal (TInternalMessageAction messageAction, TAuthentication authentication, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
      Support.Argument.Types.Select (authentication);
    }
    #endregion
  };
  //---------------------------//

  //----- TFactorySiblingMessageInternal
  public class TFactorySiblingMessageInternal : TMessageInternal
  {
    #region Constructor
    public TFactorySiblingMessageInternal (TInternalMessageAction messageAction, TChild child, TTypeInfo typeInfo)
      : base (TResource.TModule.Factory, messageAction, TNode.CreateDefault, typeInfo)
    {
      Node.SelectRelationSibling (child);
    }
    #endregion
  };
  //---------------------------//

  //----- TMessageInternal
  public class TMessageInternal : TMessage<TResource.TModule, TInternalMessageAction, TSupport<TInternalArgument<TArgumentInternal>>, TNode>
  {
    #region Constructor
    public TMessageInternal (TResource.TModule moduleName, TInternalMessageAction messageAction, TNode node, TTypeInfo typeInfo)
      : base (moduleName, messageAction, TDefault.SupportInternal, node, typeInfo)
    {
    }
    #endregion
  };
  //---------------------------//

}  // namespace