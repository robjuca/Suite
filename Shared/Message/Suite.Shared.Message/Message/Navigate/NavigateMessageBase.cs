/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Message
{
  public abstract class TNavigateMessage
  {
    #region Data
    public enum TAction
    {
      None,
      Request,
      NavigateTo,
    };

    public enum TWhere
    {
      None,
      Collection,
      Factory,
      Database,
      Support,
      Report,
    };

    public enum TSender
    {
      None,
      Shell,
      Bag,
    };
    #endregion

    #region Property
    public TAction Action
    {
      get;
      private set;
    }

    public TWhere Where
    {
      get;
      private set;
    }

    public TSender Sender
    {
      get;
      private set;
    }

    public Type TypeNavigateTo
    {
      get;
      private set;
    }

    public bool IsActionRequest
    {
      get
      {
        return (Action == TAction.Request);
      }
    }

    public bool IsActionNavigateTo
    {
      get
      {
        return (Action == TAction.NavigateTo);
      }
    }
    #endregion

    #region Constructor
    public TNavigateMessage (TAction action, TSender sender, TWhere where)
    {
      Action = action;
      Where = where;
      Sender = sender;
    }

    public TNavigateMessage (TAction action, TSender sender, TWhere where, Type typeNavigateTo)
    {
      Action = action;
      Where = where;
      Sender = sender;
      TypeNavigateTo = typeNavigateTo;
    }
    #endregion

    #region Members
    public bool IsSender (TSender sender)
    {
      return (Sender == sender);
    }

    public bool IsWhere (TWhere where)
    {
      return (Where == where);
    }
    #endregion
  };
  //---------------------------//

}  // namespace