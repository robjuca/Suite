/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
using System.Collections.Generic;
//---------------------------//

namespace Server.Models.Infrastructure
{
  public class TSummary
  {
    #region Property
    public TCategory Category
    {
      get;
      private set;
    }

    public Dictionary<string, int> GadgetCount
    {
      get;
      private set;
    }

    public int TotalCount
    {
      get;
      private set;
    }

    public bool ZapDisable
    {
      get;
      set;
    }

    public bool ZapBusy
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    TSummary ()
    {
      Category = TCategory.None;

      // matrix 4x4
      GadgetCount = new Dictionary<string, int> ()
      {
        {"minimini", 0}, // r1c1
        {"minismall", 0},
        {"minilarge", 0},
        {"minibig", 0},
        {"smallmini", 0}, // r2c1
        {"smallsmall", 0},
        {"smalllarge", 0},
        {"smallbig", 0},
        {"largemini", 0}, // r3c1
        {"largesmall", 0},
        {"largelarge", 0},
        {"largebig", 0},
        {"bigmini", 0}, // r4c1
        {"bigsmall", 0},
        {"biglarge", 0},
        {"bigbig", 0}, // r4c4
      };

      TotalCount = 0;

      ZapDisable = false;
      ZapBusy = false;
    }
    #endregion

    #region Members
    public void Select (TCategory category)
    {
      Category = category;
    }

    public void Select (string horizontalStyle, string verticalStyle)
    {
      string key = horizontalStyle + verticalStyle;

      if (GadgetCount.ContainsKey (key)) {
        GadgetCount [key] += 1;
        TotalCount++;
      }
    }

    public void CopyFrom (TSummary alias)
    {
      if (alias.NotNull ()) {
        Category = alias.Category;
        TotalCount = alias.TotalCount;
        ZapDisable = alias.ZapDisable;
        ZapBusy = alias.ZapBusy;

        foreach (var item in alias.GadgetCount) {
          GadgetCount [item.Key] = item.Value;
        }
      }
    }
    #endregion

    #region Static
    public static TSummary CreateDefault => new TSummary ();
    #endregion
  };
  //---------------------------//

}  // namespace