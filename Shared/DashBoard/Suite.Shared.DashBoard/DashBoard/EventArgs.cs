/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;

using rr.Library.Types;

using Shared.Types;
using Shared.ViewModel;
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

    public Guid Id
    {
      get;
      set;
    }

    public TReportData ReportData
    {
      get;
    }
    #endregion

    #region Constructor
    TDashBoardEventArgs ()
    {
      SourcePosition = TPosition.CreateDefault;
      TargetPosition = TPosition.CreateDefault;

      Id = Guid.Empty;

      ReportData = TReportData.CreateDefault;
    }
    #endregion

    #region Static
    public static TDashBoardEventArgs CreateDefault => new TDashBoardEventArgs (); 
    #endregion
  };
  //---------------------------//

}  // namespace