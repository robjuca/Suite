/*----------------------------------------------------------------
  Copyright (C) 2001 R&R Soft - All rights reserved.
  author: Roberto Oliveira Jucá    
----------------------------------------------------------------*/

//----- Include
using System;
//---------------------------//

namespace Shared.Types
{
  public sealed class TFilterInfo
  {
    #region Property
    public TFilterEnabled FilterEnabled
    {
      get;
      private set;
    }

    public TFilterPicture FilterPicture
    {
      get;
      private set;
    }

    public string What
    {
      get;
      set;
    }
    #endregion

    #region Constructor
    TFilterInfo ()
    {
      FilterEnabled = TFilterEnabled.all;
      FilterPicture = TFilterPicture.all;

      What = string.Empty;
    }
    #endregion

    #region Members
    public void Select (TFilterInfo alias)
    {
      if (alias != null) {
        FilterEnabled = alias.FilterEnabled;
        FilterPicture = alias.FilterPicture;
        What = alias.What;
      }
    }

    public void SelectFilterEnabled (string filter)
    {
      FilterEnabled = (TFilterEnabled) Enum.Parse (typeof (TFilterEnabled), filter);
    }

    public void SelectFilterPicture (string filter)
    {
      FilterPicture = (TFilterPicture) Enum.Parse (typeof (TFilterPicture), filter);
    }

    public bool ValidateSearch ()
    {
      bool res = true;

      if (string.IsNullOrEmpty (What.Trim ())) {
        res = false;
      }

      if (What.Trim ().Length < 3) {
        res = false;
      }

      return (res);
    }

    public void CleanSearch ()
    {
      What = string.Empty;
    }
    #endregion

    #region Property
    public static TFilterInfo CreateDefault => new TFilterInfo (); 
    #endregion
  };
  //---------------------------//

}  // namespace