/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;
using rr.Library.Message;

using Shared.Types;
//---------------------------//

namespace Shared.Message
{
  //----- TDefault
  public static class TDefault
  {
    #region TSupport
    public static TSupport<TModuleArgument<TArgumentModule>> SupportModule => new TSupport<TModuleArgument<TArgumentModule>> (ArgumentModule);
    #endregion

    #region TArgument
    public static TModuleArgument<TArgumentModule> ArgumentModule => new TModuleArgument<TArgumentModule> (new TArgumentModule ());
    #endregion
  };
  //---------------------------//

  //----- TArgumentModule
  public class TArgumentModule : TArgumentTypesDefault
  {
    #region Property
    public TAuthentication Authentication
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TArgumentModule ()
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

  //----- TArgumentTypes
  public class TArgumentTypesDefault
  {
    #region Property
    public TDatabaseAuthentication ConnectionData
    {
      get;
      private set;
    }

    public TRequestData RequestData
    {
      get;
      private set;
    }

    public TReportData ReportData
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.IEntityAction EntityAction
    {
      get;
      private set;
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
      private set;
    }

    public TStyleInfo HorizontalStyle
    {
      get;
    }

    public TStyleInfo VerticalStyle
    {
      get;
    }
    #endregion

    #region Constructor
    public TArgumentTypesDefault ()
    {
      ConnectionData = TDatabaseAuthentication.CreateDefault;

      RequestData = TRequestData.CreateDefault;
      ReportData = TReportData.CreateDefault;

      EntityAction = null;
      Category = Server.Models.Infrastructure.TCategory.None;

      HorizontalStyle = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      VerticalStyle = TStyleInfo.Create (TContentStyle.Mode.Vertical);
    }
    #endregion

    #region Members
    public void Select (TRequestData data)
    {
      RequestData.CopyFrom (data);
    }

    public void Select (TReportData data)
    {
      ReportData.CopyFrom (data);
    }

    public void Select (Server.Models.Infrastructure.IEntityAction entityAction)
    {
      EntityAction = entityAction;

      if (EntityAction.NotNull ()) {
        if (EntityAction.Operation.HasOperation) {
          RequestData.Select (TWhat.Database);
        }
      }
    }
    
    public void Select (Server.Models.Infrastructure.TCategory category)
    {
      Category = category;
    }

    public bool IsOperationCategory (Server.Models.Infrastructure.TCategory category)
    {
      return (EntityAction.IsNull () ? false : EntityAction.Operation.IsCategory (category));
    }

    public bool IsOperation (Server.Models.Infrastructure.TOperation operation)
    {
      return (EntityAction.IsNull () ? false : EntityAction.Operation.IsOperation (operation));
    }

    public bool IsOperation (Server.Models.Infrastructure.TOperation operation, Server.Models.Infrastructure.TExtension extension)
    {
      return (EntityAction.IsNull () ? false : EntityAction.Operation.IsOperation (operation, extension));
    }

    public void CopyFrom (TArgumentTypesDefault alias)
    {
      if (alias != null) {
        ConnectionData.CopyFrom (alias.ConnectionData);

        RequestData.CopyFrom (alias.RequestData);
        ReportData.CopyFrom (alias.ReportData);

        EntityAction = alias.EntityAction;
        Category = alias.Category;
      }
    }
    #endregion
  };
  //---------------------------//

  //----- TModuleArgument<B>
  public class TModuleArgument<B> 
  {
    #region Property
    public B Types
    {
      get;
      private set;
    }

    public TModuleArgs Args
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TModuleArgument (B argumentTypes)
    {
      Types = argumentTypes;
      Args = new TModuleArgs ();
    }
    #endregion
  };
  //---------------------------//

  //----- TModuleArgs
  public class TModuleArgs
  {
    #region Property
    public TWhere Where
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TModuleArgs ()
    {
      Where = TWhere.None;
    }
    #endregion

    #region Members
    public void Select (TWhere where)
    {
      Where = where;
    }

    public bool IsWhere (TWhere where)
    {
      return (Where == where);
    }
    #endregion
  };
  //---------------------------//

  //----- TInternalArgument<B> 
  public class TInternalArgument<B> 
  {
    #region Property
    public B Types
    {
      get;
      private set;
    }

    public TInternalArgs Args
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TInternalArgument (B argumentTypes)
    {
      Types = argumentTypes;
      Args = new TInternalArgs ();
    }
    #endregion
  };
  //---------------------------//

  //----- TInternalArgs
  public class TInternalArgs
  {
    #region Property
    public TWhere Where
    {
      get;
      private set;
    }

    public TNavigateForm NavigateForm
    {
      get;
      private set;
    }

    public string PropertyName
    {
      get;
      private set;
    }

    public Guid Id
    {
      get;
      private set;
    }

    public object Param1
    {
      get;
      private set;
    }

    public object Param2
    {
      get;
      private set;
    }
    #endregion

    #region Constructor
    public TInternalArgs ()
    {
      Where = TWhere.None;
      NavigateForm = TNavigateForm.None;
    }
    #endregion

    #region Members
    public void Select (TNavigateForm navigate)
    {
      NavigateForm = navigate;
    }

    public void Select (TWhere where)
    {
      Where = where;
    }

    public void Select (string propertyName)
    {
      PropertyName = propertyName;
    }

    public void Select (Guid id)
    {
      Id = id;
    }

    public void Select (object param1, object param2)
    {
      Param1 = param1;
      Param2 = param2;
    }

    public bool IsWhere (TWhere where)
    {
      return (Where.Equals (where));
    }

    public bool IsNavigate (TNavigateForm navigate)
    {
      return (NavigateForm.Equals (navigate));
    }
    #endregion
  };
  //---------------------------//

}  // namespace
