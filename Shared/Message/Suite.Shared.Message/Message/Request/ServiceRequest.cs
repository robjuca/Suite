/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using rr.Library.Types;

using Shared.Types;
//---------------------------//

namespace Shared.Message
{
  public class TServiceRequest
  {
    #region Property
    public bool IsDatabase
    {
      get
      {
        return (RequestData.IsWhat (TWhat.Database));
      }
    }

    public TChild Child
    {
      get
      {
        return (m_Child);
      }
    }

    public TModuleName ModuleName
    {
      get
      {
        return (m_ModuleName);
      }
    }

    public TDatabaseAuthentication DatabaseAuthentication
    {
      get; 
    }

    public TRequestData RequestData
    {
      get;
    }

    public Server.Models.Infrastructure.IEntityAction EntityAction
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get
      {
        return (EntityAction.Equals (null) ? Server.Models.Infrastructure.TCategory.None : EntityAction.Operation.CategoryType.Category);
      }
    }
    #endregion

    #region Constructor
    TServiceRequest ()
    {
      m_Child = TChild.None;
      m_ModuleName = TModuleName.None;

      DatabaseAuthentication = TDatabaseAuthentication.CreateDefault;
      RequestData = TRequestData.CreateDefault;

      EntityAction = null;
    }

    TServiceRequest (TMessageModule message)
      : this () => Select (message);
    #endregion

    #region Members
    public void Select (TChild child)
    {
      m_Child = child;
    }

    public void Select (TDatabaseAuthentication database)
    {
      DatabaseAuthentication.CopyFrom (database);
    }

    public void Select (TRequestData data)
    {
      RequestData.CopyFrom (data);
    }

    public void Select (TMessageModule message)
    {
      if (message != null) {
        Select (message.Node.Child);
        Select (message.Support.Argument.Types.RequestData);
        Select (message.Support.Argument.Types.ConnectionData);

        EntityAction = message.Support.Argument.Types.EntityAction;

        m_ModuleName = message.Node.ModuleName;
      }
    }

    public void Request (TMessageModule message)
    {
      message.Node.SelectRelationModule (Child, ModuleName);
      message.Support.Argument.Types.RequestData.CopyFrom (RequestData);
    }

    public bool IsCategory (Server.Models.Infrastructure.TCategory category)
    {
      return (EntityAction.Equals (null) ? false : EntityAction.Operation.IsCategory (category));
    }
    #endregion

    #region Property
    static public TServiceRequest CreateDefault => (new TServiceRequest ());

    static public TServiceRequest Create (TMessageModule message) => (new TServiceRequest (message));
    #endregion

    #region Fields
    TChild                                            m_Child;
    TModuleName                                       m_ModuleName;
    #endregion
  };
  //---------------------------//

}  // namespace