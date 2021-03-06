﻿/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.Types;
//---------------------------//

namespace Shared.DashBoard
{
  public sealed class TDashBoardEventArgs : EventArgs
  {
    #region Property
    public TPosition SourcePosition
    {
      get;
      private set;
    }

    public TPosition TargetPosition
    {
      get;
      private set;
    }

    public TSize BoardSize
    {
      get; 
    }

    public Server.Models.Infrastructure.TCategory Category
    {
      get;
      private set;
    }

    public Guid Id
    {
      get;
      set;
    }

    public TReportData ReportData
    {
      get;
    }

    public TStyleInfo HorizontalStyleInfo
    {
      get;
    }

    public TStyleInfo VerticalStyleInfo
    {
      get;
    }
    #endregion

    #region Constructor
    TDashBoardEventArgs ()
    {
      SourcePosition = TPosition.CreateDefault;
      TargetPosition = TPosition.CreateDefault;

      BoardSize = TSize.CreateDefault;

      Category = Server.Models.Infrastructure.TCategory.None;
      Id = Guid.Empty;

      ReportData = TReportData.CreateDefault;

      HorizontalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Horizontal);
      VerticalStyleInfo = TStyleInfo.Create (TContentStyle.Mode.Vertical);
    }
    #endregion

    #region Members
    public void Select (Server.Models.Infrastructure.TCategory category)
    {
      Category = category;
    } 
    #endregion

    #region Static
    public static TDashBoardEventArgs CreateDefault => new TDashBoardEventArgs (); 
    #endregion
  };
  //---------------------------//

}  // namespace